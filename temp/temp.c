#include <stdio.h>

#default MAX_SIZE 256

void main() {
	unsigend char mainArr[MAX_SIZE] = {0};
	char mainArrOffset = 0;
	while (mainArr[mainArrOffset])	{
		printf("%c", mainArr[mainArrOffset]);
	}
	mainArr[mainArrOffset] += 10;
	while (mainArr[mainArrOffset])	{
		mainArrOffset += 1;
		mainArr[mainArrOffset] += 7;
		mainArrOffset -= 1;
		mainArr[mainArrOffset] -= 1;
	}
	mainArrOffset += 1;
	mainArr[mainArrOffset] += 2;
	printf("%c", mainArr[mainArrOffset]);
	mainArrOffset -= 1;
	mainArr[mainArrOffset] += 10;
	while (mainArr[mainArrOffset])	{
		mainArrOffset += 1;
		mainArr[mainArrOffset] += 3;
		mainArrOffset -= 1;
		mainArr[mainArrOffset] -= 1;
	}
	mainArrOffset += 1;
	mainArr[mainArrOffset] -= 1;
	printf("%c", mainArr[mainArrOffset]);
	mainArr[mainArrOffset] += 7;
	mainArrOffset -= 1;
	mainArr[mainArrOffset] += 2;
	while (mainArr[mainArrOffset])	{
		mainArrOffset += 1;
		printf("%c", mainArr[mainArrOffset]);
		mainArrOffset -= 1;
		mainArr[mainArrOffset] -= 1;
	}
	mainArrOffset += 1;
	mainArr[mainArrOffset] += 3;
	printf("%c", mainArr[mainArrOffset]);
	mainArrOffset += 2;
	mainArr[mainArrOffset] += 3;
	while (mainArr[mainArrOffset])	{
		mainArrOffset -= 1;
		mainArr[mainArrOffset] += 11;
		mainArrOffset += 1;
		mainArr[mainArrOffset] -= 1;
	}
	mainArrOffset -= 1;
	mainArr[mainArrOffset] -= 1;
	printf("%c", mainArr[mainArrOffset]);
	mainArr[mainArrOffset] += 8;
	mainArrOffset += 1;
	mainArr[mainArrOffset] += 4;
	while (mainArr[mainArrOffset])	{
		mainArrOffset -= 1;
		mainArr[mainArrOffset] += 11;
		mainArrOffset += 1;
		mainArr[mainArrOffset] -= 1;
	}
	mainArrOffset -= 1;
	mainArr[mainArrOffset] += 3;
	printf("%c", mainArr[mainArrOffset]);
	mainArrOffset -= 1;
	printf("%c", mainArr[mainArrOffset]);
	mainArr[mainArrOffset] += 3;
	printf("%c", mainArr[mainArrOffset]);
	mainArr[mainArrOffset] -= 6;
	printf("%c", mainArr[mainArrOffset]);
	mainArr[mainArrOffset] -= 8;
	printf("%c", mainArr[mainArrOffset]);
	while (mainArr[mainArrOffset])	{
		mainArr[mainArrOffset] -= 1;
	}
	mainArr[mainArrOffset] += 3;
	while (mainArr[mainArrOffset])	{
		mainArrOffset -= 1;
		mainArr[mainArrOffset] += 11;
		mainArrOffset += 1;
		mainArr[mainArrOffset] -= 1;
	}
	mainArrOffset -= 1;
	printf("%c", mainArr[mainArrOffset]);

}
