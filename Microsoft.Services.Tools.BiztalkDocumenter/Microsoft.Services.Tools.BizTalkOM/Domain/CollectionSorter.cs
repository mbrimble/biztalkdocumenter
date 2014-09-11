
namespace Microsoft.Services.Tools.BizTalkOM
{
	using System;
	using System.Collections;
	using System.Diagnostics;
	using System.Reflection;

	
	/// <summary>
	/// Contains all commonly used error strings. These are usually used when
	/// generating exceptions.
	/// </summary>
	internal class ErrorStrings
	{
		public const string EmptyValue =
			"Value of '{0}' is empty or null. Please define a non-empty value.";

		public const string UnassignedProperty =
			"Using unassigned property '{0}'";

	}

	/// <summary>
	/// Provides the functionality to sort a collection class.
	/// </summary>
	public class CollectionSorter : IComparer 
	{

		#region Error strings

		const string ErrPropertyNotFound =
			"The propery or field with name '{0}' was not found on type {1}. " +
			"Please check the spelling of the name. The name is case sensitive.";

		const string ErrInterfaceNotFound =
			"Property or field type does not support the IComparable interface";

		#endregion

		#region Private fields

		string[] m_columnOrder;
		SortOrder m_sortOrder;

		#endregion

		/// <summary>
		/// Defines the sort order
		/// </summary>
		public enum SortOrder 
		{

			#region enum SortOrder

			/// <summary>
			/// Ascending sort order
			/// </summary>
			Ascending,
			/// <summary>
			/// Descending sort order
			/// </summary>
			Descending

			#endregion
		}

		/// <summary>
		/// Constructor used to specify the columns to sort on
		/// </summary>
		/// <param name="columnOrder">
		/// The columns on which to sort on. The columns correspond
		/// to property or field names of an item in the collection
		/// </param>
		/// <remarks>
		/// The default sort order is Ascending
		/// </remarks>
		public CollectionSorter( params string[] columnOrder ) 
		{

			#region Constructor with column order as input

			Debug.Assert( columnOrder != null, string.Format( ErrorStrings.EmptyValue, "columnOrder" ) );
			m_sortOrder = SortOrder.Ascending;
			m_columnOrder = columnOrder;

			#endregion
		}

		/// <summary>
		/// Constructor used to specify the columns to sort on and the sort order
		/// </summary>
		/// <param name="sortOrder">
		/// A value of Ascending implies that the values in the specified column should 
		/// be sorted in ascending order, from lowest value to highest value.
		/// A value of Decending implies that the values in the specified column should 
		/// be sorted in descending order, from highest value to lowest value.
		/// </param>
		/// <param name="columnOrder">
		/// The columns on which to sort on. The columns correspond
		/// to property or field names of an item in the collection
		/// </param>
		public CollectionSorter( SortOrder sortOrder, params string[] columnOrder ) 
		{

			#region Constructor with column order and sort order direction

			Debug.Assert( columnOrder != null, string.Format( ErrorStrings.EmptyValue, "columnOrder" ) );
			m_sortOrder = sortOrder;
			m_columnOrder = columnOrder;

			#endregion
		}

		/// <summary>
		/// Compares two items in the collection
		/// </summary>
		/// <param name="x">
		/// The first item to compare
		/// </param>
		/// <param name="y">
		/// The second item to compare
		/// </param>
		/// <returns>
		/// Less than zero, if x less than y
		/// Zero, if x = y
		/// Greater than zero, if x greater than y
		/// </returns>
		/// <implements>
		/// IComparer.Compare
		/// </implements>
		public int Compare( object x, object y ) 
		{
            
			#region Implementation Compare

			Debug.Assert( m_columnOrder != null, string.Format( ErrorStrings.EmptyValue, "columnOrder" ) );

			int iRet = 0;
			for( int i = 0; i < m_columnOrder.Length; i++ ) 
			{
				object valueX, valueY;

				// Determine the value of the current column to sort on in x.

				// Determine if the column to sort on is a property of x
				PropertyInfo property = x.GetType().GetProperty( m_columnOrder[i] );
				if( property != null ) 
				{
					valueX = property.GetValue( x, null );
				}
				else 
				{
					// Otherwise, determine if the column to sort on is a field of x
					FieldInfo field = x.GetType().GetField( m_columnOrder[i] );
					if( field == null ) 
					{
						throw new ApplicationException(
							string.Format( ErrPropertyNotFound, m_columnOrder[i], x.GetType().FullName ) );
					}
					valueX = field.GetValue( x );
				}

				// Determine the value of the current column to sort on in y.

				// Determine if the column to sort on is a property of y
				property = y.GetType().GetProperty( m_columnOrder[i] );
				if( property != null ) 
				{
					valueY = property.GetValue( y, null );
				}
				else 
				{
					// Otherwise, determine if the column to sort on is a field of y
					FieldInfo field = y.GetType().GetField( m_columnOrder[i] );
					if( field == null ) 
					{
						throw new ApplicationException(
							string.Format( ErrPropertyNotFound, m_columnOrder[i], y.GetType().FullName ) );
					}
					valueY = field.GetValue( y );
				}

				// Do comparison
				IComparable comparer = (valueX as IComparable);
				if( comparer == null ) 
				{
					throw new ArgumentException(
						ErrInterfaceNotFound, x.GetType().FullName + "." +  m_columnOrder[i] );
				}
				iRet = comparer.CompareTo( valueY );

				if( iRet != 0 ) break;
			}

			// Special case if descending sort order
			if( iRet != 0 && m_sortOrder == SortOrder.Descending ) 
			{
				iRet = -iRet;
			}

			return iRet;

			#endregion
		}

	}
}
