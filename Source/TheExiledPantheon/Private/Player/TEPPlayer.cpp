// Fill out your copyright notice in the Description page of Project Settings.


#include "Player/TEPPlayer.h"
#include "Components/InputComponent.h"
#include "AbilitySystemComponent.h"
#include "Player/TEPPlayerState.h"
#include "GameFramework/SpringArmComponent.h"
#include "Camera/CameraComponent.h"

ATEPPlayer::ATEPPlayer()
{
	PrimaryActorTick.bCanEverTick = true;

	SpringArm = CreateDefaultSubobject<USpringArmComponent>("SpringArm");
	SpringArm->SetupAttachment(GetMesh());

	Camera = CreateDefaultSubobject<UCameraComponent>("Camera");
	Camera->AttachToComponent(SpringArm, FAttachmentTransformRules::KeepRelativeTransform);

	SpringArm->TargetArmLength = ArmLength;
	SpringArm->SetRelativeRotation(FRotator(ArmYRotation, 0.f, 0.f));
}

void ATEPPlayer::BeginPlay()
{
	Super::BeginPlay();
}

void ATEPPlayer::Tick(const float DeltaTime)
{
	Super::Tick(DeltaTime);
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

void ATEPPlayer::SetArmLength(const float Length)
{
	ArmLength = Length;

	SpringArm->TargetArmLength = ArmLength;
}

float ATEPPlayer::GetArmLength() const
{
	return ArmLength;
}
