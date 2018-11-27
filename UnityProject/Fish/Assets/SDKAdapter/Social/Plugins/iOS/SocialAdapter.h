//
//  SocialAdapter.h
//  Unity-iPhone
//
//  Created by snowcold on 2017/10/11.
//

#ifndef SocialAdapter_h
#define SocialAdapter_h
#import <StoreKit/StoreKit.h>

@interface SocialAdapter : NSObject<SKStoreProductViewControllerDelegate>

- (void)productViewControllerDidFinish:(SKStoreProductViewController*)viewController;

@end

void ShareTextWithUrl (NSString* title, NSString* msg, NSString* url);

void ShareTitleAndImage(NSString *title, NSString *imagePath);

#endif /* SocialAdapter_h */
