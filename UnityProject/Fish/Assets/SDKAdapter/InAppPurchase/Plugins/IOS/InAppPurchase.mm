//
//  InAppPurchase.m
//  VPNMaster
//
//  Created by Sherlock on 2018/2/2.
//  Copyright © 2018年 陈浩. All rights reserved.
//

#import "InAppPurchase.h"

@interface InAppPurchase()

@property(nonatomic,strong)NSString *productId;

@property(nonatomic,assign)BOOL isQueryProductInfo;// 标记是否查询商品信息

@end

@implementation InAppPurchase

+(instancetype)sharedInstance {
    static InAppPurchase *instance = nil;
    static dispatch_once_t onceToken;
    
    dispatch_once(&onceToken, ^{
        instance = [[InAppPurchase alloc] init];
    });
    
    return instance;
}


#pragma mark 查询所有的商品信息
extern "C" {
    void _queryAllProductsInfo(const char * productstr) // c#中的string类型 在mm里是 char *
    {
        NSString *products = [[NSString alloc] initWithUTF8String:productstr];
        NSArray *productIdArr = [NSJSONSerialization JSONObjectWithData:[products dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingMutableContainers error:nil];
        [[InAppPurchase sharedInstance] requestArrayProductData:productIdArr];
        
    }
}

extern "C" {
    void _buyProductWithId(const char * productId)
    {
        [[InAppPurchase sharedInstance] buyProductWithId:[NSString stringWithUTF8String:productId]];
    }
}

-(void)buyProductWithId:(NSString *)productId {
    self.productId = productId;
    if([SKPaymentQueue canMakePayments]) {
        [self requestProductData:productId];
    } else {
        //不允许程序内付费
        const char* str = [self generateCstrWith:NO ErrorMsg:@"Can not be paid in app" productId:self.productId];
        UnitySendMessage("PurchaseMgr", "OnPurchaseFailed", str);
    }
}

-(const char *)generateCstrWith:(BOOL) success ErrorMsg:(NSString *)msg productId:(NSString *)productId {
    NSDictionary *dic = @{@"Success":[NSNumber numberWithBool:success],@"ErrorMsg":msg,@"productId":productId};
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dic options:NSJSONWritingPrettyPrinted error:nil];
    return  [[[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding] UTF8String];
}

-(void)requestArrayProductData:(NSArray *)productArr {
    self.isQueryProductInfo = YES;
    
    NSSet *productSet = [NSSet setWithArray:productArr];
    
    SKProductsRequest *request = [[SKProductsRequest alloc] initWithProductIdentifiers:productSet];
    
    request.delegate = self;
    [request start];
}

-(void)requestProductData:(NSString *)productId {
    self.isQueryProductInfo = NO;
    
    NSArray *productArr = @[productId];
    
    NSSet *productSet = [NSSet setWithArray:productArr];
    
    SKProductsRequest *request = [[SKProductsRequest alloc] initWithProductIdentifiers:productSet];
    
    request.delegate = self;
    [request start];
}

#pragma mark - SKProductsRequestDelegate
- (void)productsRequest:(SKProductsRequest *)request didReceiveResponse:(SKProductsResponse *)response {
    NSArray *productArr = response.products;
    if (productArr.count == 0) {
        //没有该商品
        const char* str = [self generateCstrWith:NO ErrorMsg:@"No products found" productId:(self.productId == nil) ? @"" : self.productId];
        
        if (self.isQueryProductInfo) {
            UnitySendMessage("PurchaseMgr", "OnInitPurchaseFailed", str);

        } else {
            UnitySendMessage("PurchaseMgr", "OnPurchaseFailed", str);

        }
        return;
    }
    
    //只是查询信息
    if (self.isQueryProductInfo) {
        NSMutableArray *productInfoArr = [NSMutableArray new];
        for (SKProduct *pro in productArr) {
            [productInfoArr addObject:@{@"priceLocale":pro.priceLocale.localeIdentifier,@"productIdentifier":pro.productIdentifier}];
        }
        
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:productInfoArr options:NSJSONWritingPrettyPrinted error:nil];
        NSString *str = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        UnitySendMessage("PurchaseMgr", "OnInitPurchaseResult", [str UTF8String]);

        
    } else {
        SKProduct *p = nil;
        for (SKProduct *pro in productArr) {
            if ([pro.productIdentifier isEqualToString:self.productId]) {
                p = pro;
            }
        }
        
        SKPayment *payment = [SKPayment paymentWithProduct:p];
        
        //发送内购请求
        [[SKPaymentQueue defaultQueue] addPayment:payment];
    }

}

#pragma mark - SKRequestDelegate
-(void)requestDidFinish:(SKRequest *)request {
    
}

-(void)request:(SKRequest *)request didFailWithError:(NSError *)error {
    const char* str = [self generateCstrWith:NO ErrorMsg:@"SKRequest Failed" productId:self.productId];
    //获取商品失败
	if (self.isQueryProductInfo) {
	  UnitySendMessage("PurchaseMgr", "OnInitPurchaseFailed", str);

	} else {
	  UnitySendMessage("PurchaseMgr", "OnPurchaseFailed", str);

	}

}

-(void)paymentQueue:(SKPaymentQueue *)queue updatedTransactions:(NSArray<SKPaymentTransaction *> *)transactions {
    for (SKPaymentTransaction *tran in transactions) {
        self.productId =  tran.payment.productIdentifier; //获取productId

       switch (tran.transactionState) {
        case SKPaymentTransactionStatePurchased: //交易完成
            {
                //扣款成功
                const char* str = [self generateCstrWith:YES ErrorMsg:@"" productId:self.productId];

                UnitySendMessage("PurchaseMgr", "OnPurchaseSuccess", str);
                [[SKPaymentQueue defaultQueue] finishTransaction:tran];

                
            }
            break;
            
        case SKPaymentTransactionStatePurchasing: //商品添加进列表
            
            break;
            
        case SKPaymentTransactionStateRestored: //恢复购买

            break;
            
        case SKPaymentTransactionStateFailed: //交易失败
            {
                //商品交易失败
                NSString *errorDetail = nil;
                
                if (tran.error != nil) {
                    switch (tran.error.code) {
                        case SKErrorUnknown:
                        {
                            errorDetail = @"未知错误，您可能在使用越狱手机";
                        }
                            break;
                            
                        case SKErrorClientInvalid:
                        {
                            errorDetail = @"当前苹果账户无法购买商品";
                        }
                            
                            break;
                            
                        case SKErrorPaymentCancelled:
                        {
                            errorDetail = @"订单已经取消";
                            
                        }
                            break;
                            
                        case SKErrorPaymentNotAllowed:
                        {
                            errorDetail = @"当前苹果设备无法购买商品";
                        }
                            break;
                            
                        case SKErrorStoreProductNotAvailable:
                        {
                            errorDetail = @"当前商品不可用";
                        }
                            break;
                            
                        default:
                        {
                            errorDetail = @"未知错误";
                            
                        }
                            break;
                    }
                    
                }
                 //商品交易失败
                const char* str = [self generateCstrWith:NO ErrorMsg:errorDetail productId:self.productId];
                UnitySendMessage("PurchaseMgr", "OnPurchaseFailed", str);
                
                [[SKPaymentQueue defaultQueue] finishTransaction:tran];
                
            }
            break;
            
        default:
            break;
        }
    }
}








@end
