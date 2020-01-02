#version 330 core

out vec4 FragColor;

in vec2 TexCoords;
uniform sampler2DMS screenTexture;
uniform int viewport_height;
uniform int viewport_width;

void main()
{
    // texelFetch requires a vec of ints for indexing
    ivec2 vpCoords = ivec2(viewport_width, viewport_height);
    vpCoords.x = int(vpCoords.x * TexCoords.x);
    vpCoords.y = int(vpCoords.y * TexCoords.y);

    vec4 sample1 = texelFetch(screenTexture, vpCoords, 0);
    vec4 sample2 = texelFetch(screenTexture, vpCoords, 1);
    vec4 sample3 = texelFetch(screenTexture, vpCoords, 2);
    vec4 sample4 = texelFetch(screenTexture, vpCoords, 3);

    FragColor = (sample1 + sample2 + sample3 + sample4) / 4.0;
}