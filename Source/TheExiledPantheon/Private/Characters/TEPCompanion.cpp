// Fill out your copyright notice in the Description page of Project Settings.


#include "Characters/TEPCompanion.h"

#include "AbilitySystem/TEPAbilitySystemComponent.h"
#include "AbilitySystem/TEPAttributeSet.h"

ATEPCompanion::ATEPCompanion()
{
	AbilitySystemComponent = CreateDefaultSubobject<UTEPAbilitySystemComponent>("AbilitySystemComponent");
	AttributeSet = CreateDefaultSubobject<UTEPAttributeSet>("AttributeSet");
}

void ATEPCompanion::BeginPlay()
{
	Super::BeginPlay();

	AbilitySystemComponent->InitAbilityActorInfo(this, this);
}
