// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "ChessTile.generated.h"

class UBoxComponent;
class UPaperSprite;
class UPaperSpriteComponent;

UCLASS()
class THEEXILEDPANTHEON_API AChessTile : public AActor
{
	GENERATED_BODY()
	
public:	
	AChessTile();
	virtual void Tick(float DeltaTime) override;
	
	UFUNCTION()
	void SetClickedTile() const;

	UFUNCTION()
	void SetHoveredTile() const;

protected:
	virtual void BeginPlay() override;

	UPROPERTY(VisibleAnywhere, BlueprintReadOnly)
	UBoxComponent* BoxCollision;

	UPROPERTY(EditAnywhere, Category = Sprite)
	UPaperSprite* ChessTileDefaultSprite;

	UPROPERTY(EditAnywhere, Category = Sprite)
	UPaperSprite* ChessTileHoveredSprite;

	UPROPERTY(EditAnywhere, Category = Sprite)
	UPaperSprite* ChessTileClickedSprite;

	UPROPERTY(VisibleAnywhere)
	UPaperSpriteComponent* SpriteComponent;

	UFUNCTION()
	virtual void OnMouseOver(UPrimitiveComponent* TouchedComponent);

	UFUNCTION()
	virtual void OnMouseExit(UPrimitiveComponent* TouchedComponent);
};
