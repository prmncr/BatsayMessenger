﻿using System;

namespace BatsayMessenger.VkClasses;

public class Group : Member
{
	public Group(long id, string name, Uri photoUri) : base(id, name, photoUri)
	{
	}

	public long AbsId => -Id;
}