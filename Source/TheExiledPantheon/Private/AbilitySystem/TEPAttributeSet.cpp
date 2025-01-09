// Fill out your copyright notice in the Description page of Project Settings.


#include "AbilitySystem/TEPAttributeSet.h"
#include "Net/UnrealNetwork.h"

UTEPAttributeSet::UTEPAttributeSet()
{
	InitHealth(10.f);
	InitMaxHealth(10.f);
	InitSpeed(2.f);
	InitMaxSpeed(2.f);
}
