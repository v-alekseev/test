// UnitTestC++.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include "testUT.h"

using namespace std;

namespace TestVit
{


	int GetCountCard(int type)
	{
		return (int)type;

	}


}

int _tmain(int argc, _TCHAR* argv[])
{
	cout << "type is " << TestVit::GetCountCard(TestVit::CardTypes::ExtraSmall) << "\n";
	cout << "type is " << TestVit::GetCountCard(TestVit::CardTypes::Small) << "\n";
	cout << "type is " << TestVit::GetCountCard(TestVit::CardTypes::Normal) << "\n";
	//int k;

	//cin >> k; 

	return 0;
}