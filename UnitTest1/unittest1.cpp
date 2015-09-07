#include "stdafx.h"
#include "CppUnitTest.h"
#include "testUT.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

using namespace TestVit;

namespace UnitTest1
{		
	TEST_CLASS(UnitTest1)
	{
	public:
		
		TEST_METHOD(CountCards)
		{
			Assert::AreEqual(TestVit::GetCountCard(TestVit::CardTypes::ExtraSmall), (int)TestVit::CardTypes::ExtraSmall);
		}

		TEST_METHOD(test)
		{
			Assert::AreEqual(32, 32);
		}
	};
}