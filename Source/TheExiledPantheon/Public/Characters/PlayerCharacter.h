// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "PlayerCharacter.generated.h"

class UInputMappingContext;
class UInputAction;
class AChessTile;

UCLASS()
class THEEXILEDPANTHEON_API APlayerCharacter : public ACharacter
{
	GENERATED_BODY()

public:
	APlayerCharacter();
	virtual void Tick(float DeltaTime) override;
	virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;

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
