// Fill out your copyright notice in the Description page of Project Settings.


#include "Combat/Chessboard.h"
#include "Combat/ChessTile.h"
#include "GameFramework/GameSession.h"

AChessboard::AChessboard()
{
	PrimaryActorTick.bCanEverTick = true;
}

void AChessboard::BeginPlay()
{
	Super::BeginPlay();

	for (int i = 0; i < Height; i++)
	{
		TileYPos = TileYSpacing * -i;
		if (i % 2 == 0)
		{
			SpawnRowOfTiles(MainTile, AlternateTile);
		}
		else
		{
			SpawnRowOfTiles(AlternateTile, MainTile);
		}
	}
}

void AChessboard::SpawnTileAtLocation(const TSubclassOf<AChessTile> TileToSpawn) const
{
	GetWorld()->SpawnActor<AChessTile>(TileToSpawn, FVector(TileXPos, TileYPos, 0.f), FRotator::ZeroRotator);
}

void AChessboard::SpawnRowOfTiles(const TSubclassOf<AChessTile> FirstTile, const TSubclassOf<AChessTile> SecondTile)
{
	for (int i = 0; i < Width; i++)
	{
		TileXPos = TileXSpacing * i;
		if (i % 2 == 0)
		{
			SpawnTileAtLocation(FirstTile);
		}
		else
		{
			SpawnTileAtLocation(SecondTile);
		}
	}
}

void AChessboard::Tick(const float DeltaTime)
{
	Super::Tick(DeltaTime);
}

