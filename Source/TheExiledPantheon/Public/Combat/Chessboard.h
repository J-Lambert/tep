// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "Chessboard.generated.h"

class AChessTile;

UCLASS()
class THEEXILEDPANTHEON_API AChessboard : public AActor
{
	GENERATED_BODY()
	
public:	
	AChessboard();
	virtual void Tick(float DeltaTime) override;
protected:
	virtual void BeginPlay() override;

	// Number of rows on the chessboard
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Size")
	uint8 Height = 5;

	// Number of columns on the chessboard
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Size")
	uint8 Width = 5;

	UPROPERTY(VisibleAnywhere, Category = "Position")
	double TileXPos = 0;

	UPROPERTY(VisibleAnywhere, Category = "Position")
	double TileYPos = 0;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Spacing")
	float TileXSpacing = 160.f;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Spacing")
	float TileYSpacing = 160.f;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Tiles")
	TSubclassOf<AChessTile> MainTile;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Tiles")
	TSubclassOf<AChessTile> AlternateTile;

	UFUNCTION(BlueprintCallable)
	void SpawnTileAtLocation(TSubclassOf<AChessTile> TileToSpawn) const;

	UFUNCTION(BlueprintCallable)
	void SpawnRowOfTiles(TSubclassOf<AChessTile> FirstTile, TSubclassOf<AChessTile> SecondTile);
};
