// Fill out your copyright notice in the Description page of Project Settings.


#include "Combat/ChessTile.h"
#include "Components/BoxComponent.h"
#include "PaperSpriteComponent.h"

AChessTile::AChessTile()
{
	PrimaryActorTick.bCanEverTick = true;
	BoxCollision = CreateDefaultSubobject<UBoxComponent>(TEXT("BoxCollision"));
	RootComponent = BoxCollision;

	BoxCollision->SetBoxExtent(FVector(79.f, 79.f, 5.f));

	SpriteComponent = CreateDefaultSubobject<UPaperSpriteComponent>(TEXT("SpriteComponent"));
	SpriteComponent->SetupAttachment(GetRootComponent());
	SpriteComponent->SetRelativeRotation(FRotator(0.f, 0.f, 90.f));
	SpriteComponent->SetSprite(ChessTileDefaultSprite);

}

void AChessTile::BeginPlay()
{
	Super::BeginPlay();

	SpriteComponent->OnBeginCursorOver.AddDynamic(this, &AChessTile::OnMouseOver);
	SpriteComponent->OnEndCursorOver.AddDynamic(this, &AChessTile::OnMouseExit);
}

void AChessTile::Tick(const float DeltaTime)
{
	Super::Tick(DeltaTime);
}

void AChessTile::OnMouseOver(UPrimitiveComponent* TouchedComponent)
{
	SetHoveredTile();
}

void AChessTile::OnMouseExit(UPrimitiveComponent* TouchedComponent)
{
	SpriteComponent->SetSprite(ChessTileDefaultSprite);
}

void AChessTile::SetClickedTile() const
{
	SpriteComponent->SetSprite(ChessTileClickedSprite);
}

void AChessTile::SetHoveredTile() const
{
	SpriteComponent->SetSprite(ChessTileHoveredSprite);
}