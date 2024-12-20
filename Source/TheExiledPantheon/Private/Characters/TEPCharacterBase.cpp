// Fill out your copyright notice in the Description page of Project Settings.


#include "Characters/TEPCharacterBase.h"

ATEPCharacterBase::ATEPCharacterBase()
{
	PrimaryActorTick.bCanEverTick = false;
}

UAbilitySystemComponent* ATEPCharacterBase::GetAbilitySystemComponent() const
{
	return AbilitySystemComponent;
}

void ATEPCharacterBase::BeginPlay()
{
	Super::BeginPlay();
	
}

