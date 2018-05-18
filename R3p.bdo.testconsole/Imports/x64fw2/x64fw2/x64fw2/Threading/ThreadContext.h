#pragma once
#include <vector>
#include <memory>

namespace Infinity
{
	namespace Utilities
	{
		namespace Threading
		{
			class Thread;
			class ThreadContext
			{

			public:
				static ThreadContext& GetInstance();

			public:
				explicit ThreadContext();
				~ThreadContext();
				std::vector<std::shared_ptr<Thread>> GetThreads() const;
				std::vector<std::shared_ptr<Thread>> SuspendAllThreads() const;
			};
		}
	}
}
