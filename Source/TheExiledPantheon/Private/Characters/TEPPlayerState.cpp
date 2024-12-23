// Fill out your copyright notice in the Description page of Project Settings.


#include "Characters/TEPPlayerState.h"

#include "AbilitySystem/TEPAbilitySystemComponent.h"
#include "AbilitySystem/TEPAttributeSet.h"

ATEPPlayerState::ATEPPlayerState()
{
	AbilitySystemComponent = CreateDefaultSubobject<UTEPAbilitySystemComponent>("AbilitySystemComponent");
	AttributeSet = CreateDefaultSubobject<UTEPAttributeSet>("AttributeSet");
}

UAbilitySystemComponent* ATEPPlayerState::GetAbilitySystemComponent() const
{
	return AbilitySystemComponent;
}
