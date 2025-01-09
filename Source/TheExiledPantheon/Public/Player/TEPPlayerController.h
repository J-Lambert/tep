// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/PlayerController.h"
#include "TEPPlayerController.generated.h"

struct FInputActionValue;
class UInputMappingContext;
class UInputAction;
class AChessTile;

UCLASS()
class THEEXILEDPANTHEON_API ATEPPlayerController : public APlayerController
{
	GENERATED_BODY()
public:
	ATEPPlayerController();
	virtual void PlayerTick(float DeltaTime) override;
protected:
	virtual void BeginPlay() override;
	virtual void SetupInputComponent() override;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = Controls)
	float ZoomSpeed = 25.f;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = Controls)
	float ZoomMin = 0.f;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = Controls)
	float ZoomMax = 1000.f;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = Controls)
	float MoveSpeed = 10.f;
private:
	UPROPERTY(EditAnywhere, Category = Input)
	TObjectPtr<UInputMappingContext> CombatContext;

	UPROPERTY(EditAnywhere, Category = Input)
	TObjectPtr<UInputAction> ClickAction;

	UPROPERTY(EditAnywhere, Category = Input)
	TObjectPtr<UInputAction> ScrollAction;

	UPROPERTY(EditAnywhere, Category = Input)
	TObjectPtr<UInputAction> MoveAction;

	UPROPERTY(VisibleInstanceOnly)
	AChessTile* ClickedTile;

	void LeftMouseClicked();
	void LeftMouseReleased();
	void Scroll(const FInputActionValue& InputActionValue);
	void MoveCamera(const FInputActionValue& InputActionValue);
};
