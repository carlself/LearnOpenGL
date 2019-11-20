#pragma once
#include <assimp/Importer.hpp>
#include <assimp/scene.h>
#include <assimp/postprocess.h>

#include "Mesh.h"

#include <vector>
#include <string>

using namespace std;

class Model
{
public:
	Model(const char* path)
	{
		loadModel(path);
	}

	void Draw(Shader shader)
	{
		for (int i = 0; i < meshes.size(); i++)
		{
			meshes[i].Draw(shader);
		}
	}

private:
	vector<Mesh> meshes;
	string directory;

	void loadModel(string path)
	{
		Assimp::Importer importer;
		const aiScene *scene = importer.ReadFile(path, aiProcess_Triangulate | aiProcess_FlipUVs);

		if (!scene || scene->mFlags &AI_SCENE_FLAGS_INCOMPLETE || !scene->mRootNode)
		{
			cout << "ERROR::ASSIMP::" << importer.GetErrorString() << endl;
			return;
		}

		directory = path.substr(0, path.find_last_of('/'));

		processNode(scene->mRootNode, scene);
	}

	void processNode(aiNode *node, const aiScene *scene)
	{
		for (int i = 0; i < node->mNumMeshes; i++)
		{
			aiMesh *mesh = scene->mMeshes[node->mMeshes[i]];
			meshes.push_back(processMesh(mesh, scene));
		}

		for (int i = 0; i < node->mNumChildren; i++)
		{
			processNode(node->mChildren[i], scene);
		}
	}

	Mesh processMesh(aiMesh *mesh, const aiScene *scene)
	{
		vector<Vertex> vertices;
		vector<unsigned int> indices;
		vector<Texture> textures;
	}

	vector<Texture> loadMaterialTextures(aiMaterial *mat, aiTextureType type, string typeName)
	{

	}
};