﻿using System.Text;
using ModuleNet.ModuleNet.Attributes;
using TdsHelper.Abstractions;
using TdsHelper.Models;
using TdsHelper.TypeControllers;

namespace TdsHelper
{
    [Injectable]
    public class ControllerFactory
    {
        private readonly ControllerCollection _controllers;

        public ControllerFactory(ControllerCollection controllers)
        {
            _controllers = controllers;
        }

        public string GetPostgresDataType(Column column)
        {
            return GetController(column).ToPostgresTypeString();
        }

        private IController GetController(Column column)
        {
            return _controllers.GetController(column);
        }

        public static ControllerFactory Default { get; set; }
    }
}
