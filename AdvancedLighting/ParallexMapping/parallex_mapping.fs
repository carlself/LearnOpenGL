#version 330 core
out vec4 FragColor;

in VS_OUT {
    vec3 FragPos;
    vec2 TexCoords;
    vec3 TangentLightPos;
    vec3 TangentViewPos;
    vec3 TangentFragPos;
} fs_in;

uniform sampler2D diffuseMap;
uniform sampler2D normalMap;
uniform sampler2D depthMap;
uniform float heightScale;

// vec2 ParallexMapping(vec2 texCoords, vec3 viewDir)
// {
//     float height = texture(depthMap, texCoords).r;
//     vec2 p = viewDir.xy * (height * height_scale);
//     return texCoords - p;
// }

// vec2 ParallexMapping(vec2 texCoords, vec3 viewDir) // steep parallex mapping
// {
//     const float numLayers = 10;
//     float layerDepth = 1.0 / numLayers;
//     float currentLayerDepth = 0.0;
//     vec2 P = viewDir.xy * heightScale;
//     vec2 deltaTexCoords = P / numLayers;

//     vec2 currentTexCoords = texCoords;
//     float currentDepthMapValue = texture(depthMap, currentTexCoords).r;

//     while(currentLayerDepth < currentDepthMapValue)
//     {
//         currentTexCoords -= deltaTexCoords;
//         currentDepthMapValue = texture(depthMap, currentTexCoords).r;
//         currentLayerDepth += layerDepth;
//     }

//     return currentTexCoords;
// }

vec2 ParallexMapping(vec2 texCoords, vec3 viewDir) // Parallax Occlusion Mapping
{
    const float numLayers = 10;
    float layerDepth = 1.0 / numLayers;
    float currentLayerDepth = 0.0;
    vec2 P = viewDir.xy * heightScale;
    vec2 deltaTexCoords = P / numLayers;

    vec2 currentTexCoords = texCoords;
    float currentDepthMapValue = texture(depthMap, currentTexCoords).r;

    while(currentLayerDepth < currentDepthMapValue)
    {
        currentTexCoords -= deltaTexCoords;
        currentDepthMapValue = texture(depthMap, currentTexCoords).r;
        currentLayerDepth += layerDepth;
    }

    // get texture coordinates before collision (reverse operations)
    vec2 preTexCoords = currentTexCoords + deltaTexCoords;

    // get depth after and before collision for linear interpolation
    float afterDepth = currentDepthMapValue - currentLayerDepth;
    float beforeDepth = texture(depthMap, preTexCoords).r - currentLayerDepth + layerDepth;

    // interpolation of texture coordinates
    float weight = afterDepth / (afterDepth - beforeDepth);
    vec2 finalTexCoords = preTexCoords * weight + currentTexCoords * (1.0 - weight);

    return finalTexCoords;
}

void main()
{
    vec3 viewDir = normalize(fs_in.TangentViewPos - fs_in.FragPos);

    vec2 texCoords = ParallexMapping(fs_in.TexCoords, viewDir);
    if(texCoords.x > 1.0 || texCoords.y > 1.0 || texCoords.x < 0.0 || texCoords.y < 0.0)
        discard;

    vec3 texColor = texture(diffuseMap, texCoords).rgb;
    vec3 normal = texture(normalMap, texCoords).rgb;
    normal = normalize(normal * 2.0 - 1.0);

    vec3 ambient = 0.1 * texColor;

    vec3 lightDir = normalize(fs_in.TangentLightPos - fs_in.TangentFragPos);
    float diff = max(dot(lightDir, normal), 0.0);
    vec3 diffuse = diff * texColor;

    vec3 halfDir = normalize(viewDir + lightDir);
    float spec = pow(max(dot(halfDir, normal), 0.0),64.0);
    vec3 specular = spec * vec3(0.2);

    FragColor = vec4(ambient + diffuse + specular, 1.0);
}