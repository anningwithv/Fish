//
//  SocialAdapter.m
//  Unity-iPhone
//
//  Created by snowcold on 2017/10/11.
//

#import <Foundation/Foundation.h>
#import <Social/Social.h>
#import "SocialAdapter.h"
#import "CustomActivity.h"
#import "UnityAppController.h"
#import <StoreKit/StoreKit.h>

void ShareTextWithUrl(NSString* title, NSString* msg, NSString* urlData)
{
    
    NSDictionary *infoPlist = [[NSBundle mainBundle] infoDictionary];
    
    NSString *icon = [[infoPlist valueForKeyPath:@"CFBundleIcons.CFBundlePrimaryIcon.CFBundleIconFiles"] lastObject];
    
    UIImage *shareImage= [UIImage imageNamed:icon];
    
//    UIImage *image = [UIImage imageNamed:@"AppIcon"];
    NSURL *url = [NSURL URLWithString:urlData];
    NSArray *activityItems = @[title,shareImage, url];
    
    UnityAppController* unity = GetAppController();
    UIViewController* rootViewContor = unity.rootViewController;
    
    // 服务类型控制器
    UIActivityViewController *activityViewController =
    [[UIActivityViewController alloc] initWithActivityItems:activityItems applicationActivities:nil];
    activityViewController.modalInPopover = true;
    
    //activityViewController.excludedActivityTypes = @[UIActivityTypePostToVimeo ];

    UIPopoverPresentationController *popover = activityViewController.popoverPresentationController;
    
    if (popover) {
        
        popover.sourceView = unity.rootViewController.view;
    }
    
    [rootViewContor presentViewController:activityViewController animated:YES completion:nil];
    
    // 选中分享类型
    [activityViewController setCompletionWithItemsHandler:^(NSString * __nullable activityType, BOOL completed, NSArray * __nullable returnedItems, NSError * __nullable activityError){
        
        // 显示选中的分享类型
        NSLog(@"act type %@",activityType);
        
        if (completed) {
            NSLog(@"ok");
        }else {
            NSLog(@"no ok");
        }
        
    }];
}

SocialAdapter* s_Adapter;

void OpenAppStoreRatepageInner(NSString* appID)
{
    //appID = @"1292266901";
    SKStoreProductViewController* storeProductViewContorller = [[SKStoreProductViewController alloc]init];
    
    // 设置代理请求为当前控制器本身
    if (s_Adapter == nil)
    {
        s_Adapter = [[SocialAdapter alloc] init];
    }
    storeProductViewContorller.delegate = s_Adapter;
    
    UnityAppController* unity = GetAppController();
    UIViewController* rootViewContor = unity.rootViewController;
    //接着弹出VC
    [rootViewContor presentViewController:storeProductViewContorller animated:YES completion:nil];
    //最后加载应用数据
    [storeProductViewContorller loadProductWithParameters:@{SKStoreProductParameterITunesItemIdentifier:appID} completionBlock:^(BOOL result, NSError * _Nullable error) {
        if (error) {
            //handle the error
        }
    }];
}

extern "C"
{
    void ShareText2SocialPlatform(const char* title, const char* msg, const char* url)
    {
        ShareTextWithUrl([[NSString alloc] initWithUTF8String:title], [[NSString alloc] initWithUTF8String:msg], [[NSString alloc] initWithUTF8String:url]);
    }

    void OpenAppStoreRatepage(const char* appID)
    {
        OpenAppStoreRatepageInner([[NSString alloc] initWithUTF8String:appID]);
    }
}

extern "C"
{
    void ShareImage2SocialPlatform(const char* title, const char* imagePath)
    {
        ShareTitleAndImage([[NSString alloc] initWithUTF8String:title],[[NSString alloc] initWithUTF8String:imagePath]);
    }
}

void ShareTitleAndImage(NSString *title, NSString *imagePath)
{
    NSData* imageData = [[NSData alloc] initWithContentsOfURL:[NSURL fileURLWithPath:imagePath]];
    
    UIImage* image = [[UIImage alloc] initWithData:imageData];
    
    NSArray *activityItems = @[title,image];

    UnityAppController* unity = GetAppController();
    UIViewController* rootViewContor = unity.rootViewController;
    
    // 服务类型控制器
    UIActivityViewController *activityViewController =
    [[UIActivityViewController alloc] initWithActivityItems:activityItems applicationActivities:nil];
    activityViewController.modalInPopover = true;
    
    //activityViewController.excludedActivityTypes = @[UIActivityTypePostToVimeo ];
    
    UIPopoverPresentationController *popover = activityViewController.popoverPresentationController;
    
    if (popover) {
        
        popover.sourceView = unity.rootViewController.view;
    }
    
    [rootViewContor presentViewController:activityViewController animated:YES completion:nil];
    
    // 选中分享类型
    [activityViewController setCompletionWithItemsHandler:^(NSString * __nullable activityType, BOOL completed, NSArray * __nullable returnedItems, NSError * __nullable activityError){
        
        // 显示选中的分享类型
        NSLog(@"act type %@",activityType);
        
        if (completed) {
            NSLog(@"ok");
        }else {
            NSLog(@"no ok");
        }
        
    }];


}

@implementation SocialAdapter

- (void)productViewControllerDidFinish:(SKStoreProductViewController*)viewController
{
    UnityAppController* unity = GetAppController();
    UIViewController* rootViewContor = unity.rootViewController;
    [rootViewContor dismissViewControllerAnimated:YES completion:nil];
    
}

@end

