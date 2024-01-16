using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using GameCreator.Editor.Common;
using UnityEngine.UIElements;
using GameCreator.Runtime.Common;

namespace NinjutsuGames.StateMachine.Editor
{
    public class DocumentationStateMachine : VisualElement
    {
        public const string PATH_USS = EditorPaths.COMMON + "Documentation/Styles/Documentation";

        public const string NAME_SEPARATOR_SMALL = "GC-Documentation-Separator-Small";
        public const string NAME_SEPARATOR_LARGE = "GC-Documentation-Separator-Large";

        public const string NAME_HEADER = "GC-Documentation-Header";
        public const string NAME_PARAMETERS = "GC-Documentation-Parameters";
        public const string NAME_DEPENDENCIES = "GC-Documentation-Dependencies";
        public const string NAME_EXAMPLES = "GC-Documentation-Examples";
        public const string NAME_KEYWORDS = "GC-Documentation-Keywords";
        public const string NAME_RENDERING_PIPELINES = "GC-Documentation-RenderingPipelines";

        public const string NAME_ICON = "GC-Documentation-Icon";
        public const string NAME_TITLE = "GC-Documentation-Title";
        public const string NAME_DESCRIPTION = "GC-Documentation-Description";
        public const string NAME_CATEGORY = "GC-Documentation-Category";
        public const string NAME_VERSION = "GC-Documentation-Version";

        public const string NAME_PARAMETER = "GC-Documentation-Parameter";
        public const string NAME_PARAMETER_TITLE = "GC-Documentation-Parameter-Title";
        public const string NAME_PARAMETER_DESCR = "GC-Documentation-Parameter-Descr";

        public const string NAME_DEPENDENCY = "GC-Documentation-Dependency";
        public const string NAME_DEPENDENCY_ID = "GC-Documentation-Dependency-ID";
        public const string NAME_DEPENDENCY_VERSION = "GC-Documentation-Dependency-Version";

        public const string NAME_EXAMPLE_TITLE = "GC-Documentation-Example-Title";
        public const string NAME_EXAMPLE_CONTENT = "GC-Documentation-Example-Content";

        public const string NAME_KEYWORD = "GC-Documentation-Keyword";
        public const string NAME_RENDERING_PIPELINE_ON = "GC-Documentation-RenderingPipeline-On";
        public const string NAME_RENDERING_PIPELINE_OFF = "GC-Documentation-RenderingPipeline-Off";

        public const string ROW_EVEN = "gc-documentation-row-even";
        public const string ROW_ODD = "gc-documentation-row-odd";

        public const string CATEGORY_SEPARATOR = " › ";

        private InfoMessage description;
        private VisualElement parameters;
        private VisualElement examples;

        public DocumentationStateMachine(Type type)
        {
            var styleSheets = StyleSheetUtils.Load(PATH_USS);
            foreach (var styleSheet in styleSheets)
            {
                this.styleSheets.Add(styleSheet);
            }

            Update(type);
        }

        public void Update(Type type)
        {
            if(description != null) Remove(description);
            if(parameters != null) Remove(parameters);
            if(examples != null) Remove(examples);
            
            description = IncludeDescription(type, false);
            parameters = IncludeParameters(type, false);
            examples = IncludeExamples(type, false);
            
            if(description != null) Add(description);
            if(parameters != null) Add(parameters);
            if(examples != null) Add(examples);
        }

        private InfoMessage IncludeDescription(Type type, bool prefixSeparator)
        {
            var attribute = type
                .GetCustomAttributes<DescriptionAttribute>(true)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(attribute?.Description)) return null;

            var label = new InfoMessage(attribute.Description);
            if (prefixSeparator) IncludeSeparator(parent);
            return label;
        }

        private VisualElement IncludeParameters(Type type, bool prefixSeparator)
        {
            var parameters = new List<ParameterAttribute>(
                type.GetCustomAttributes<ParameterAttribute>(true)
            );

            if (parameters.Count == 0) return null;

            var container = new VisualElement {name = NAME_PARAMETERS};

            for (var i = 0; i < parameters.Count; ++i)
            {
                var content = new VisualElement {name = NAME_PARAMETER};
                content.AddToClassList(i % 2 == 0 ? ROW_EVEN : ROW_ODD);

                content.Add(new Label
                {
                    name = NAME_PARAMETER_TITLE,
                    text = parameters[i].Name
                });

                content.Add(new Label
                {
                    name = NAME_PARAMETER_DESCR,
                    text = parameters[i].Description
                });

                container.Add(content);
            }

            if (prefixSeparator) this.IncludeSeparator(parent);
            return container;
        }

        private VisualElement IncludeExamples(Type type, bool prefixSeparator)
        {
            var examples = new List<ExampleAttribute>(
                type.GetCustomAttributes<ExampleAttribute>(true)
            );

            if (examples.Count == 0) return null;

            var container = new VisualElement {name = NAME_EXAMPLES};

            for (var i = 0; i < examples.Count; i++)
            {
                container.Add(new Label
                {
                    name = NAME_EXAMPLE_TITLE,
                    text = $"Example {i + 1}"
                });

                container.Add(new Label
                {
                    name = NAME_EXAMPLE_CONTENT,
                    text = examples[i].Content
                });
            }

            if (prefixSeparator) this.IncludeSeparator(parent);
            return container;
        }

        private void IncludeSeparator(VisualElement parent)
        {
            var separator = new VisualElement {name = NAME_SEPARATOR_SMALL};
            parent.Add(separator);
        }
    }
}