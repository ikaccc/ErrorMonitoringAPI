using System;
using System.Collections;

namespace ExceptionHandler.API.Common
{
    public static class Assumption
    {
        public static void FailValidation(string msg, string argumentName)
        {
            throw new ArgumentException(msg, argumentName);
        }

        public static bool FailValidationBool(bool val)
        {
            return val;
        }
        public static void AssertIsInterface<T>() where T : class
        {
            if (typeof(T).IsInterface)
                return;
            Assumption.FailValidation("Generic type parameter should be an interface type.", typeof(T).Name);
        }

        public static bool AssertTrue(bool testedValue, string parameterName)
        {
            if (!testedValue)
                Assumption.FailValidation("Argument should be equal to true", parameterName);
            return testedValue;
        }

        public static bool AssertFalse(bool testedValue, string parameterName)
        {
            if (testedValue)
                Assumption.FailValidation("Argument should be equal to false", parameterName);
            return testedValue;
        }

        public static T AssertNotEqual<T>(T testedValue, T compareToValue, string parameterName) where T : IEquatable<T>
        {
            if (testedValue.Equals(compareToValue))
                Assumption.FailValidation("Argument should not be equal to " + (object)compareToValue, parameterName);
            return testedValue;
        }

        public static T AssertEqual<T>(T testedValue, T compareToValue, string parameterName) where T : IEquatable<T>
        {
            if (!testedValue.Equals(compareToValue))
                Assumption.FailValidation("Argument should be equal to " + (object)compareToValue, parameterName);
            return testedValue;
        }

        public static T AssertNotNull<T>(T value, string parameterName) where T : class
        {
            if ((object)value == null)
                Assumption.FailValidation("Argument should not be NULL", parameterName);
            return value;
        }

        public static string AssertNotNullOrEmpty(string value, string parameterName)
        {
            if (value == null)
                Assumption.FailValidation("Argument should not be NULL.", parameterName);
            if (string.IsNullOrEmpty(value))
                Assumption.FailValidation("Argument should not be an empty string.", parameterName);
            return value;
        }

        public static IEnumerable AssertNotNullOrEmpty(IEnumerable value, string parameterName)
        {
            if (value == null)
                Assumption.FailValidation("Argument should not be NULL.", parameterName);
            bool flag = false;
            IEnumerator enumerator = value.GetEnumerator();
            try
            {
                if (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    flag = true;
                }
            }
            finally
            {
                (enumerator as IDisposable)?.Dispose();
            }
            if (!flag)
                Assumption.FailValidation("Argument should not be an empty enumerable.", parameterName);
            return value;
        }

        public static string AssertEqual(string value, string expectedValue, bool ignoreCase, string parameterName)
        {
            if (value == null && expectedValue == null)
                return (string)null;
            if (value == null || expectedValue == null)
                Assumption.FailValidation("Argument is expected to be equal to " + expectedValue + ", while it is actually: " + value + ".", parameterName);
            if (string.Compare(value, expectedValue, ignoreCase) != 0)
                Assumption.FailValidation("Argument is expected to be equal to " + expectedValue + ", while it is actually: " + value + ".", parameterName);
            return value;
        }

        public static string AssertNotNullOrWhiteSpace(string value, string parameterName)
        {
            if (value == null || value.Trim().Length == 0)
                Assumption.FailValidation("Parameter should not be null or white space.", parameterName);
            return value;
        }

        public static bool AssertNotNullOrWhiteSpaceBool(string value)
        {
            return value != null && value.Trim().Length != 0;
        }

        public static Guid AssertNotEmpty(Guid value, string parameterName)
        {
            if (value == Guid.Empty)
                Assumption.FailValidation("Parameter should not be an empty Guid.", parameterName);
            return value;
        }

        public static int AssertGreaterThan(int value, int expectedValue, string parameterName)
        {
            if (value <= expectedValue)
                Assumption.FailValidation("Argument should be greater than " + (object)expectedValue, parameterName);
            return value;
        }

        public static double AssertGreaterThan(double value, double expectedValue, string parameterName)
        {
            if (value <= expectedValue)
                Assumption.FailValidation("Argument should be greater than " + (object)expectedValue, parameterName);
            return value;
        }

        public static DateTimeOffset AssertGreaterThan(DateTimeOffset value, DateTimeOffset expectedValue, string parameterName)
        {
            if (value <= expectedValue)
                Assumption.FailValidation("Argument should be greater than " + (object)expectedValue, parameterName);
            return value;
        }

        public static int AssertGreaterThanOrEqual(int value, int expectedValue, string parameterName)
        {
            if (value < expectedValue)
                Assumption.FailValidation("Argument should be greater than or equal to " + (object)expectedValue, parameterName);
            return value;
        }

        public static double AssertGreaterThanOrEqual(double value, double expectedValue, string parameterName)
        {
            if (value < expectedValue)
                Assumption.FailValidation("Argument should be greater than or equal to " + (object)expectedValue, parameterName);
            return value;
        }

        public static DateTimeOffset AssertGreaterThanOrEqual(DateTimeOffset value, DateTimeOffset expectedValue, string parameterName)
        {
            if (value < expectedValue)
                Assumption.FailValidation("Argument should be greater than or equal to " + (object)expectedValue, parameterName);
            return value;
        }

        public static int AssertLessThan(int value, int expectedValue, string parameterName)
        {
            if (value >= expectedValue)
                Assumption.FailValidation("Argument should be less than " + (object)expectedValue, parameterName);
            return value;
        }

        public static double AssertLessThan(double value, double expectedValue, string parameterName)
        {
            if (value >= expectedValue)
                Assumption.FailValidation("Argument should be less than " + (object)expectedValue, parameterName);
            return value;
        }

        public static DateTimeOffset AssertLessThan(DateTimeOffset value, DateTimeOffset expectedValue, string parameterName)
        {
            if (value >= expectedValue)
                Assumption.FailValidation("Argument should be less than " + (object)expectedValue, parameterName);
            return value;
        }

        public static int AssertLessThanOrEqual(int value, int expectedValue, string parameterName)
        {
            if (value > expectedValue)
                Assumption.FailValidation("Argument should be less than or equal to " + (object)expectedValue, parameterName);
            return value;
        }

        public static double AssertLessThanOrEqual(double value, double expectedValue, string parameterName)
        {
            if (value > expectedValue)
                Assumption.FailValidation("Argument should be less than or equal to " + (object)expectedValue, parameterName);
            return value;
        }

        public static DateTimeOffset AssertLessThanOrEqual(DateTimeOffset value, DateTimeOffset expectedValue, string parameterName)
        {
            if (value > expectedValue)
                Assumption.FailValidation("Argument should be less than or equal to " + (object)expectedValue, parameterName);
            return value;
        }

        public static T AssertNotNullAndOfType<T>(object value, string parameterName) where T : class
        {
            if (value == null)
                Assumption.FailValidation(string.Format("Expected argument '{0}' can not be NULL.", (object)parameterName), parameterName);
            T obj = value as T;
            if ((object)obj == null)
                Assumption.FailValidation(string.Format("Expected argument of type " + (object)typeof(T) + ", but was " + (object)value.GetType(), (object)typeof(T), (object)value.GetType()), parameterName);
            return obj;
        }
    }
}
