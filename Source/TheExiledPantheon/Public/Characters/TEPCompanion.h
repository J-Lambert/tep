// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Characters/TEPCharacterBase.h"
#include "TEPCompanion.generated.h"

/**
 * 
 */
UCLASS()
class THEEXILEDPANTHEON_API ATEPCompanion : public ATEPCharacterBase
{
	GENERATED_BODY()

public:
	ATEPCompanion();

protected:
	virtual void BeginPlay() override;
};
