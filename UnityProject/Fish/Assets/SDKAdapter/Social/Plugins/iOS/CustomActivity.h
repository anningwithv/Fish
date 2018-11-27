//
//  CustomActivity.h
//  Unity-iPhone
//
//  Created by snowcold on 2017/10/11.
//

#ifndef CustomActivity_h
#define CustomActivity_h

#import <UIKit/UIKit.h>

@interface CustomActivity : UIActivity

@property (nonatomic, copy) NSString * title;

@property (nonatomic, strong) UIImage * image;

@property (nonatomic, strong) NSURL * url;

@property (nonatomic, copy) NSString * type;

@property (nonatomic, strong) NSArray * shareContexts;

-(instancetype)initWithTitie:(NSString *)title withActivityImage:(UIImage *)image withUrl:(NSURL *)url withType:(NSString *)type  withShareContext:(NSArray *)shareContexts;
@end

#endif /* CustomActivity_h */
