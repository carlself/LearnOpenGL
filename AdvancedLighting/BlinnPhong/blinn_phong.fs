#version 330 core

out vec4 FragColor;

struct Material {
    sampler2D diffuse;
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

in VS_OUT {
    vec3 FragPos;
    vec3 Normal;
    vec2 TexCoords;
} fs_in;


uniform vec3 viewPos;
uniform bool blinn;

void main()
{
    vec3 diffuseTexColor = vec3(texture(material.diffuse, fs_in.TexCoords));

    vec3 norm = normalize(fs_in.Normal);
    vec3 lightDir = normalize(light.position - fs_in.FragPos);
    float diff = max(dot(norm, lightDir), 0.0);

    vec3 viewDir = normalize(viewPos - fs_in.FragPos);
    float spec = 0.0;
    if(blinn)
    {
        vec3 halfDir = normalize(viewDir + lightDir);
        spec = pow(max(dot(norm, halfDir), 0.0), material.shininess);
    }
    else
    {
        vec3 relfectDir = reflect(-lightDir, norm);
        spec = pow(max(dot(viewDir, relfectDir), 0.0), material.shininess);
    }

    vec3 ambient = diffuseTexColor * light.ambient;
    vec3 diffuse = diff * diffuseTexColor * light.diffuse;
    vec3 specular = spec * material.specular * light.specular;

    FragColor = vec4(ambient + diffuse + specular, 1.0);
}