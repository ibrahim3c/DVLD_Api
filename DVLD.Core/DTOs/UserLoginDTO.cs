﻿using System.ComponentModel.DataAnnotations;

namespace DVLD.Core.DTOs;

public class UserLoginDTO
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = default!;


    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;
}
