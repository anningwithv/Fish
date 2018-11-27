//
//  RCPurchaserInfo.m
//  Purchases
//
//  Created by Jacob Eiting on 9/30/17.
//  Copyright © 2018 RevenueCat, Inc. All rights reserved.
//

#import "RCPurchaserInfo.h"

@interface RCPurchaserInfo ()

@property (nonatomic) NSDictionary<NSString *, NSDate *> *expirationDatesByProduct;
@property (nonatomic) NSDictionary<NSString *, NSObject *> *expirationDateByEntitlement;
@property (nonatomic) NSSet<NSString *> *nonConsumablePurchases;
@property (nonatomic) NSString *originalApplicationVersion;
@property (nonatomic) NSDictionary *originalData;
@property (nonatomic) NSDate * _Nullable requestDate;
@end

static NSDateFormatter *dateFormatter;
static dispatch_once_t onceToken;

@implementation RCPurchaserInfo

- (instancetype _Nullable)initWithData:(NSDictionary *)data
{
    if (self = [super init]) {
        if (data[@"subscriber"] == nil) {
            return nil;
        }

        self.originalData = data;

        NSDictionary *subscriberData = data[@"subscriber"];

        dispatch_once(&onceToken, ^{
            dateFormatter = [[NSDateFormatter alloc] init];
            dateFormatter.timeZone = [NSTimeZone timeZoneWithAbbreviation:@"GMT"];
            dateFormatter.dateFormat = @"yyyy-MM-dd'T'HH:mm:ssZ";
            dateFormatter.locale = [NSLocale localeWithLocaleIdentifier:@"en_US_POSIX"];
        });
        self.requestDate = [dateFormatter dateFromString:(NSString *)data[@"request_date"]];

        NSDictionary *subscriptions = subscriberData[@"subscriptions"];
        if (subscriptions == nil) {
            return nil;
        }

        self.expirationDatesByProduct = [self parseExpirationDate:subscriptions];

        NSDictionary *entitlements = subscriberData[@"entitlements"];
        self.expirationDateByEntitlement = [self parseExpirationDate:entitlements];

        NSDictionary<NSString *, id> *otherPurchases = subscriberData[@"other_purchases"];
        self.nonConsumablePurchases = [NSSet setWithArray:[otherPurchases allKeys]];

        NSString *originalApplicationVersion = subscriberData[@"original_application_version"];
        self.originalApplicationVersion = [originalApplicationVersion isKindOfClass:[NSNull class]] ? nil : originalApplicationVersion;

    }
    return self;
}

- (NSDictionary<NSString *, NSDate *> *)parseExpirationDate:(NSDictionary<NSString *, NSDictionary *> *)expirationDates
{
    NSMutableDictionary<NSString *, NSObject *> *dates = [NSMutableDictionary new];

    for (NSString *identifier in expirationDates) {
        id dateString = expirationDates[identifier][@"expires_date"];

        if ([dateString isKindOfClass:NSString.class]) {
            NSDate *date = [dateFormatter dateFromString:(NSString *)dateString];

            if (date != nil) {
                dates[identifier] = date;
            }
        } else {
            dates[identifier] = [NSNull null];
        }
    }

    return [NSDictionary dictionaryWithDictionary:dates];
}

- (NSSet<NSString *> *)allPurchasedProductIdentifiers
{
    return [self.nonConsumablePurchases setByAddingObjectsFromArray:self.expirationDatesByProduct.allKeys];
}

- (NSSet<NSString *> *)activeKeys:(NSDictionary<NSString *, NSObject *> *)dates
{
    NSMutableSet *activeSubscriptions = [NSMutableSet setWithCapacity:dates.count];

    for (NSString *identifier in dates) {
        NSDate *dateOrNull = (NSDate *)dates[identifier];
        if ([dateOrNull isKindOfClass:NSNull.class] || [self isAfterReferenceDate:dateOrNull]) {
            [activeSubscriptions addObject:identifier];
        }
    }

    return [NSSet setWithSet:activeSubscriptions];
}

- (BOOL)isAfterReferenceDate:(NSDate *)date {
    NSDate *referenceDate = self.requestDate ?: [NSDate date];
    return [date timeIntervalSinceDate:referenceDate] > 0;
}

- (NSSet<NSString *> *)activeSubscriptions
{
    return [self activeKeys:self.expirationDatesByProduct];
}

- (NSDate * _Nullable)latestExpirationDate
{
    NSDate *maxDate = nil;

    for (NSDate *date in self.expirationDatesByProduct.allValues) {
        if (date.timeIntervalSince1970 > maxDate.timeIntervalSince1970) {
            maxDate = date;
        }
    }

    return maxDate;
}

- (NSDate *)expirationDateForProductIdentifier:(NSString *)productIdentifier
{
    return self.expirationDatesByProduct[productIdentifier];
}

- (NSSet<NSString *> *)activeEntitlements
{
    return [self activeKeys:self.expirationDateByEntitlement];
}

- (NSDate * _Nullable)expirationDateForEntitlement:(NSString *)entitlementId
{
    NSObject *dateOrNull = self.expirationDateByEntitlement[entitlementId];
    return [dateOrNull isKindOfClass:NSNull.class] ? nil : (NSDate *)dateOrNull;
}


- (NSDictionary * _Nonnull)JSONObject {
    return self.originalData;
}

@end
