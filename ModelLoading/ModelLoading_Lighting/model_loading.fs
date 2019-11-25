#version 330 core

in vec3 Normal;
in vec2 TexCoords;
in vec3 FragPos;

struct Material
{
    sampler2D diffuse_texture1;
    sampler2D specular_texture1;

    float shininess;
};

uniform Material material;

struct PointLight
{
    vec3 position;
    
    float constant;
    float linear;
    float quadratic;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

#define NR_POINT_LIGHTS 2
uniform PointLight pointLights[NR_POINT_LIGHTS];

uniform vec3 viewPos;

out vec4 FragColor;
vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);
void main()
{
    vec3 texColor = vec3(texture(material.diffuse_texture1, TexCoords));
    FragColor = vec4(texColor, 1.0);

    vec3 normal = normalize(Normal);
    vec3 viewDir = normalize(viewPos - FragPos);

    vec3 result = vec3(0.0);

    for(int i = 0; i < NR_POINT_LIGHTS; i++)
    {
        result += CalcPointLight(pointLights[i], normal, FragPos, viewDir);
    }

    FragColor = vec4(result, 1.0);
}

vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - FragPos);
    float diff = max(dot(lightDir, normal), 0.0);

    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(reflectDir, viewDir), 0.0), material.shininess);

    vec3 ambient = light.ambient * vec3(texture(material.diffuse_texture1, TexCoords));
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse_texture1, TexCoords));
    vec3 specular = light.specular * spec * vec3(texture(material.specular_texture1, TexCoords));

    float distance = length(light.position - fragPos);
    float attention = 1.0 / (light.constant + light.linear * distance + light.quadratic * distance * distance);

    ambient *= attention;
    diffuse *= attention;
    specular *= attention;

    return (ambient  + diffuse + specular);
}