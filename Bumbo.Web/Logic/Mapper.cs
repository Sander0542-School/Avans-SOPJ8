namespace Bumbo.Web.Logic
{
    public class Mapper
    {
        /// <summary>
        /// Iterates over all the <c>currentObject</c>'s parameters and updates the <c>newObject</c> has a different non-null value for it.
        /// <c>newObject</c> parameters which do not overlap are ignored.
        /// Values which are the same are not updated to prevent EF from setting <c>currentObject</c>'s state to modified.
        /// </summary>
        /// <param name="currentObject">The object you want to map to</param>
        /// <param name="newObject">The object which contains the updated values</param>
        /// <returns><c>currentObject</c> with the updated values from <c>newObject</c></returns>
        public static object EfObjectMapper(object currentObject, object newObject)
        {
            var properties = currentObject.GetType().GetProperties();
            foreach (var property in properties)
            {
                var currentValue = property.GetValue(currentObject);
                var newValue = newObject.GetType().GetProperty(property.Name)?.GetValue(newObject);

                if (newValue != null && (currentValue == null || !currentValue.Equals(newValue)))
                {
                    property.SetValue(currentObject, newValue);
                }
            }

            return currentObject;
        }
    }
}