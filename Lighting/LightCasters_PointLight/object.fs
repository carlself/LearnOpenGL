#version 330 core 

out vec4 FragColor;

struct Material {
	sampler2D diffuse;
	sampler2D specular;

	float shininess;
};

uniform Material material;

struct Light {
	vec3 position;
	
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;

	float constant;
	float linear;
	float quadratic;
};

uniform Light light;

uniform vec3 viewPos;

in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoords;

void main()
{
	vec3 diffuseTexColor = vec3(texture(material.diffuse, TexCoords));
	vec3 norm = normalize(Normal);
	vec3 lightDir = normalize(light.position - FragPos);

	float distance = length(light.position - FragPos);
	float attention = 1.0 / (light.constant + light.linear * distance + light.quadratic * distance * distance);
	
	vec3 ambient = light.ambient * diffuseTexColor;
	ambient *= attention;

	float diff = max(dot(lightDir, norm), 0.0);
	vec3 diffuse = light.diffuse * diff * diffuseTexColor;
	diffuse *= attention;

	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 reflectDir = reflect(-lightDir, norm);
	float spec = pow(max(dot(reflectDir, viewDir), 0), material.shininess);
	vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords));
	specular *= attention;

	vec3 result = ambient  + diffuse + specular;
	FragColor = vec4(result, 1.0);
}