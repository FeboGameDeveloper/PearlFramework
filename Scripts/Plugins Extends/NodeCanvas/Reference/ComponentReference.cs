#if NODE_CANVAS


namespace Pearl.NodeCanvas
{
    public class ComponentReference<Reference> : AbstractReference<Reference>
    {
        public Reference @Component
        {
            get
            {
                if (find == null || reference == null)
                {
                    return default;
                }

                if (!find.value)
                {
                    return reference.value;
                }
                else if (reference.value == null || !_init)
                {
                    var obj = FindObj();
                    reference.value = obj != null ? obj.GetComponent<Reference>() : default;
                }

                return reference.value;
            }
            set
            {
                reference = value;
            }
        }
    }
}

#endif