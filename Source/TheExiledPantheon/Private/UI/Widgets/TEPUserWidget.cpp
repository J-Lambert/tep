// Fill out your copyright notice in the Description page of Project Settings.


#include "UI/Widgets/TEPUserWidget.h"

void UTEPUserWidget::SetWidgetController(UObject* InWidgetController)
{
	WidgetController = InWidgetController;
	WidgetControllerSet();
}
