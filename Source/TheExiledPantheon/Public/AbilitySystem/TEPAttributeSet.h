// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "AttributeSet.h"
#include "AbilitySystemComponent.h"
#include "TEPAttributeSet.generated.h"

#define ATTRIBUTE_ACCESSORS(ClassName, PropertyName) \
	GAMEPLAYATTRIBUTE_PROPERTY_GETTER(ClassName, PropertyName) \
	GAMEPLAYATTRIBUTE_VALUE_GETTER(PropertyName) \
	GAMEPLAYATTRIBUTE_VALUE_SETTER(PropertyName) \
	GAMEPLAYATTRIBUTE_VALUE_INITTER(PropertyName)

/**
 * 
 */
UCLASS()
class THEEXILEDPANTHEON_API UTEPAttributeSet : public UAttributeSet
{
	GENERATED_BODY()

public:
	UTEPAttributeSet();

	UPROPERTY(BlueprintReadOnly, Category="Vital Attributes")
	FGameplayAttributeData Health;
	ATTRIBUTE_ACCESSORS(UTEPAttributeSet, Health);

	UPROPERTY(BlueprintReadOnly, Category="Vital Attributes")
	FGameplayAttributeData MaxHealth;
	ATTRIBUTE_ACCESSORS(UTEPAttributeSet, MaxHealth);

	UPROPERTY(BlueprintReadOnly, Category="Vital Attributes")
	FGameplayAttributeData Speed;
	ATTRIBUTE_ACCESSORS(UTEPAttributeSet, Speed);

	UPROPERTY(BlueprintReadOnly, Category="Vital Attributes")
	FGameplayAttributeData MaxSpeed;
	ATTRIBUTE_ACCESSORS(UTEPAttributeSet, MaxSpeed);
};
