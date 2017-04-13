Shader "Custom/VertexColors" 
{
	Subshader
	{
		BindChannels
		{
			Bind "vertex", vertex
			Bind "color", color
		}
		Pass{}
	}

}
