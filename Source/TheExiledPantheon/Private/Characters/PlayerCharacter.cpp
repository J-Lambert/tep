// Fill out your copyright notice in the Description page of Project Settings.


#include "Characters/PlayerCharacter.h"
#include "Components/InputComponent.h"
#include "EnhancedInputComponent.h"
#include "Combat/ChessTile.h"
#include "EnhancedInputSubsystems.h"

APlayerCharacter::APlayerCharacter()
{
	PrimaryActorTick.bCanEverTick = true;
}

void APlayerCharacter::BeginPlay()
{
	Super::BeginPlay();

	if (APlayerController* PlayerController = Cast<APlayerController>(GetController()))
	{
		if (UEnhancedInputLocalPlayerSubsystem* Subsystem = ULocalPlayer::GetSubsystem<UEnhancedInputLocalPlayerSubsystem>(PlayerController->GetLocalPlayer()))
		{
			Subsystem->AddMappingContext(CombatContext, 0);
		}
	}
	
}

void APlayerCharacter::Tick(const float DeltaTime)
{
	Super::Tick(DeltaTime);

}

void APlayerCharacter::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
	Super::SetupPlayerInputComponent(PlayerInputComponent);

	if (UEnhancedInputComponent* EnhancedInputComponent = CastChecked<UEnhancedInputComponent>(PlayerInputComponent))
	{
		EnhancedInputComponent->BindAction(ClickAction, ETriggerEvent::Started, this, &APlayerCharacter::LeftMouseClicked);
		EnhancedInputComponent->BindAction(ClickAction, ETriggerEvent::Completed, this, &APlayerCharacter::LeftMouseReleased);
	}
}

void APlayerCharacter::LeftMouseClicked()
{
	FHitResult Result;
	ETraceTypeQuery VisibilityTrace = UEngineTypes::ConvertToTraceType(ECollisionChannel::ECC_Visibility);
	
	if (APlayerController* PlayerController = Cast<APlayerController>(GetController()))
	{
		PlayerController->GetHitResultUnderCursorByChannel(VisibilityTrace, true, Result);
	}

	if (Result.bBlockingHit)
	{
		ClickedTile = Cast<AChessTile>(Result.GetActor());
		ClickedTile->SetClickedTile();
	}
}

void APlayerCharacter::LeftMouseReleased()
{
	if (ClickedTile)
	{
		ClickedTile->SetHoveredTile();
		ClickedTile = nullptr;
	}
}

