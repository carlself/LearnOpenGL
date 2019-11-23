#pragma once

#include <string>
#include <cstdlib>

using namespace std;

const char* logl_root = "";
class FileSystem
{
	
private:
	typedef string(*Builder)(const std::string &);
public:
	static string GetPath(const string &path)
	{
		Builder builder = GetPathBuilder();
		return builder(path);
	}

private:
	static string GetRoot()
	{
		char *envRoot;
		size_t bufferSize;
		getenv_s(&bufferSize, nullptr,0, "LOGL_ROOT_PATH");
		if (bufferSize == 0)
		{
			envRoot = nullptr;
		}
		else
		{
			envRoot = new char[bufferSize];
			getenv_s(&bufferSize, envRoot, bufferSize, "LOGL_ROOT_PATH");
		}
		const char *givenRoot = envRoot != nullptr ? envRoot : logl_root;
		const char *root = givenRoot != nullptr ? givenRoot : "";
		return root;
	}
	static Builder GetPathBuilder()
	{
		string root = GetRoot();
		if (GetRoot() != "")
		{
			return FileSystem::GetAbsolutePath;
		}
		else
		{
			return FileSystem::GetRelativePath;
		}
	}

	static string GetAbsolutePath(const string &path)
	{
		return GetRoot() + string("/") + path;
	}

	static string GetRelativePath(const string &path)
	{
		return "../../" + path;
	}
};