using System;
using Backend.Controllers;

namespace Backend.Services;

public interface IPeopleService
{
     bool validate(People people);
}
