// Fill out your copyright notice in the Description page of Project Settings.


#include "Characters/TEPPlayer.h"
#include "Components/InputComponent.h"
#include "EnhancedInputComponent.h"
#include "Combat/ChessTile.h"
#include "EnhancedInputSubsystems.h"
#include "AbilitySystemComponent.h"
#include "Characters/TEPPlayerState.h"

ATEPPlayer::ATEPPlayer()
{
	PrimaryActorTick.bCanEverTick = true;
}

void ATEPPlayer::BeginPlay()
{
	Super::BeginPlay();

	if (const APlayerController* PlayerController = Cast<APlayerController>(GetController()))
	{
		if (UEnhancedInputLocalPlayerSubsystem* Subsystem = ULocalPlayer::GetSubsystem<UEnhancedInputLocalPlayerSubsystem>(PlayerController->GetLocalPlayer()))
		{
			Subsystem->AddMappingContext(CombatContext, 0);
		}
	}
}

void ATEPPlayer::Tick(const float DeltaTime)
{
	Super::Tick(DeltaTime);
}

void ATEPPlayer::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
	if (UEnhancedInputComponent* EnhancedInputComponent = CastChecked<UEnhancedInputComponent>(PlayerInputComponent))
	{
		EnhancedInputComponent->BindAction(ClickAction, ETriggerEvent::Started, this, &ATEPPlayer::LeftMouseClicked);
		EnhancedInputComponent->BindAction(ClickAction, ETriggerEvent::Completed, this, &ATEPPlayer::LeftMouseReleased);
	}
}

void ATEPPlayer::PossessedBy(AController* NewController)
{
	Super::PossessedBy(NewController);

	ATEPPlayerState* TEPPlayerState = GetPlayerState<ATEPPlayerState>();
	check(TEPPlayerState);
	TEPPlayerState->GetAbilitySystemComponent()->InitAbilityActorInfo(TEPPlayerState, this);
	AbilitySystemComponent = TEPPlayerState->GetAbilitySystemComponent();
	AttributeSet = TEPPlayerState->GetAttributeSet();
}

void ATEPPlayer::LeftMouseClicked()
{
	FHitResult Result;
	const ETraceTypeQuery VisibilityTrace = UEngineTypes::ConvertToTraceType(ECollisionChannel::ECC_Visibility);
	
	if (const APlayerController* PlayerController = Cast<APlayerController>(GetController()))
	{
		PlayerController->GetHitResultUnderCursorByChannel(VisibilityTrace, true, Result);
	}

	if (Result.bBlockingHit)
	{
		ClickedTile = Cast<AChessTile>(Result.GetActor());
		ClickedTile->SetClickedTile();
	}
}

void ATEPPlayer::LeftMouseReleased()
{
	if (ClickedTile)
	{
		ClickedTile->SetHoveredTile();
		ClickedTile = nullptr;
	}
}
