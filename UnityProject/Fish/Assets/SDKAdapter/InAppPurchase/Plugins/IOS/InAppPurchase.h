//
//  InAppPurchase.h
//  VPNMaster
//
//  Created by Sherlock on 2018/2/2.
//  Copyright © 2018年 陈浩. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>

@interface InAppPurchase : NSObject<SKPaymentTransactionObserver,SKProductsRequestDelegate>

+(instancetype)sharedInstance;

-(void)requestArrayProductData:(NSArray *)productArr;

-(void)buyProductWithId:(NSString *)productId;

@end
