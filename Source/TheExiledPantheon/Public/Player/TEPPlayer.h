// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Characters/TEPCharacterBase.h"
#include "TEPPlayer.generated.h"

class USpringArmComponent;
class UCameraComponent;

UCLASS()
class THEEXILEDPANTHEON_API ATEPPlayer : public ATEPCharacterBase
{
	GENERATED_BODY()

public:
	ATEPPlayer();
	virtual void Tick(float DeltaTime) override;

	virtual void PossessedBy(AController* NewController) override;

	void SetArmLength(const float Length);

	float GetArmLength() const;

protected:
	virtual void BeginPlay() override;
	
	UPROPERTY(EditDefaultsOnly, BlueprintReadWrite)
	USpringArmComponent* SpringArm;

	UPROPERTY(EditDefaultsOnly, BlueprintReadWrite)
	UCameraComponent* Camera;

	UPROPERTY(EditAnywhere, BlueprintReadWrite)
	float ArmLength = 300.f;

	UPROPERTY(EditAnywhere, BlueprintReadWrite)
	double ArmYRotation = -45.0;
};
