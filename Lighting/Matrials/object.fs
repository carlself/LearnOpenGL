#version 330 core 

out vec4 FragColor;

struct Material {
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;

	float shininess;
};

uniform Material material;

struct Light {
	vec3 position;
	
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

uniform Light light;

uniform vec3 viewPos;

in vec3 Normal;
in vec3 FragPos;


void main()
{
	vec3 ambient = light.ambient * material.ambient;
	
	vec3 norm = normalize(Normal);
	vec3 lightDir = normalize(light.position - FragPos);

	vec3 diffuse = max(dot(lightDir, norm), 0.0) * material.diffuse * light.diffuse;


	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 reflectDir = reflect(-lightDir, norm);
	vec3 specular = pow(max(dot(reflectDir, viewDir), 0), material.shininess) * material.specular * light.specular;

	vec3 result = ambient  + diffuse + specular;
	FragColor = vec4(result, 1.0);
}