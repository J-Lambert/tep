// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Characters/TEPCharacterBase.h"
#include "TEPPlayer.generated.h"

class UInputMappingContext;
class UInputAction;
class AChessTile;

UCLASS()
class THEEXILEDPANTHEON_API ATEPPlayer : public ATEPCharacterBase
{
	GENERATED_BODY()

public:
	ATEPPlayer();
	virtual void Tick(float DeltaTime) override;
	virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;

	virtual void PossessedBy(AController* NewController) override;

protected:
	virtual void BeginPlay() override;

	UPROPERTY(EditAnywhere, Category = Input)
	UInputMappingContext* CombatContext;

	UPROPERTY(EditAnywhere, Category = Input)
	UInputAction* ClickAction;

	UPROPERTY(VisibleInstanceOnly)
	AChessTile* ClickedTile;

	void LeftMouseClicked();
	void LeftMouseReleased();
};
