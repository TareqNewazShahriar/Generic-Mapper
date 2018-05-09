public static class GenericMapper
{
  /// <summary>
  ///
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="resultantObject">If the target object is already exist, pass it, mapper will assign values to this object (otherwise resultant object can be taken from the returned object)</param>
  /// <param name="skipProps">Pass an array or list of property names (strings). Pass a scaler string if only one property to skip.</param>
  /// <returns></returns>
  public static T Map<T>(object sourceobject, T resultantObject=null, dynamic _skipProps=null) where T : class
  {
      if (sourceobject == null)
          return (T)null;

      var skippedProps = new List<string>();

      if (_skipProps != null)
      {
          if (_skipProps.GetType() == typeof(string))
              skippedProps.Add(_skipProps.ToString());
          else if (_skipProps.GetType() == typeof(string[]))
              skippedProps.AddRange(_skipProps as string[]);
          else if (_skipProps.GetType() == typeof(List<string>))
              skippedProps.AddRange(_skipProps as List<string>);
      }
      skippedProps = skippedProps.Select(x => x.ToUpper()).ToList();

      var destinationType = typeof(T);
      Type sourceType = sourceobject.GetType();

      resultantObject = resultantObject ?? Activator.CreateInstance(destinationType) as T;
      Type destinitionType = resultantObject.GetType();

      List<PropertyInfo> sourceFields = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();// | BindingFlags.NonPublic | BindingFlags.Instance);
      List<PropertyInfo> destinitionsFields = destinitionType.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();// | BindingFlags.NonPublic | BindingFlags.Instance);
      PropertyInfo tempPropInfo = null;
      object value = null;
      sourceFields.ForEach(s =>
      {
          if (skippedProps.Contains(s.Name.ToUpper()))
              return;
          try
          {
              value = s.GetValue(sourceobject);
              tempPropInfo = destinitionsFields.SingleOrDefault(d => d.Name == s.Name);
              if (tempPropInfo != null)
                  tempPropInfo.SetValue(resultantObject, value);
          }
          catch { /* if problem on getting or setting, skip it. no need to re-throw. */  }
      });

      return resultantObject;
  }
}
