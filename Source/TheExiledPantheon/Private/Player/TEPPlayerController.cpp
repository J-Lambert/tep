// Fill out your copyright notice in the Description page of Project Settings.


#include "Player/TEPPlayerController.h"

#include "EnhancedInputSubsystems.h"
#include "EnhancedInputComponent.h"
#include "Combat/ChessTile.h"
#include "Player/TEPPlayer.h"

ATEPPlayerController::ATEPPlayerController()
{
}

void ATEPPlayerController::BeginPlay()
{
	Super::BeginPlay();
	check(CombatContext);

	if (UEnhancedInputLocalPlayerSubsystem* Subsystem = ULocalPlayer::GetSubsystem<UEnhancedInputLocalPlayerSubsystem>(GetLocalPlayer()))
	{
		Subsystem->AddMappingContext(CombatContext, 0);
	}
}

void ATEPPlayerController::PlayerTick(const float DeltaTime)
{
	Super::PlayerTick(DeltaTime);
}

void ATEPPlayerController::SetupInputComponent()
{
	Super::SetupInputComponent();

	if (UEnhancedInputComponent* EnhancedInputComponent = CastChecked<UEnhancedInputComponent>(InputComponent))
	{
		EnhancedInputComponent->BindAction(ClickAction, ETriggerEvent::Started, this, &ATEPPlayerController::LeftMouseClicked);
		EnhancedInputComponent->BindAction(ClickAction, ETriggerEvent::Completed, this, &ATEPPlayerController::LeftMouseReleased);
		EnhancedInputComponent->BindAction(ScrollAction, ETriggerEvent::Triggered, this, &ATEPPlayerController::Scroll);
		EnhancedInputComponent->BindAction(MoveAction, ETriggerEvent::Triggered, this, &ATEPPlayerController::MoveCamera);
	}
}

void ATEPPlayerController::LeftMouseClicked()
{
	FHitResult Result;
	const ETraceTypeQuery VisibilityTrace = UEngineTypes::ConvertToTraceType(ECollisionChannel::ECC_Visibility);
	
	GetHitResultUnderCursorByChannel(VisibilityTrace, true, Result);

	if (Result.bBlockingHit)
	{
		ClickedTile = Cast<AChessTile>(Result.GetActor());
		ClickedTile->SetClickedTile();
	}
}

void ATEPPlayerController::LeftMouseReleased()
{
	if (ClickedTile)
	{
		ClickedTile->SetHoveredTile();
		ClickedTile = nullptr;
	}
}

void ATEPPlayerController::Scroll(const FInputActionValue& InputActionValue)
{
	// Check if owning pawn can be cast to a TEPPlayer
	if (ATEPPlayer* TEPPlayer = Cast<ATEPPlayer>(GetPawn()))
	{
		// Target arm length of the spring arm is increased by the input scroll value, clamped between a minimum and maximum amount.
		TEPPlayer->SetArmLength(FMath::Clamp(TEPPlayer->GetArmLength() + (InputActionValue.Get<float>() * ZoomSpeed), ZoomMin, ZoomMax));
	}
}

void ATEPPlayerController::MoveCamera(const FInputActionValue& InputActionValue)
{
	const FVector2D InputAxisVector = InputActionValue.Get<FVector2D>();

	const float MoveAmountX = InputAxisVector.X * MoveSpeed;
	const float MoveAmountY = InputAxisVector.Y * MoveSpeed;

	if (APawn* ControlledPawn = GetPawn<APawn>())
	{
		const FVector CurrentLocation = ControlledPawn->GetActorLocation();
		ControlledPawn->SetActorLocation(FVector(CurrentLocation.X + MoveAmountX, CurrentLocation.Y + MoveAmountY, CurrentLocation.Z));
	}
}
