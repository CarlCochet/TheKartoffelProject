   M S B A M L   `   `   `  ����F  BWorldEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:WorldEditor.Helpers.ConvertersWorldEditor.Helpers.Converters  5   	   C&clr-namespace:WorldEditor.Search.ItemsWorldEditor.Search.Items  5   	   2    ,WorldEditor.Editors.Items.EffectEditorDialog   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   ?itintc,clr-namespace:WorldEditor.Helpers.Converters     5   	   7cboc&clr-namespace:WorldEditor.Search.Items     5   	   X TXceed.Wpf.Toolkit, Version=1.9.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f45xceed)http://schemas.xceed.com/wpf/xaml/toolkit  5   	     � Title$  Modifying an effect��5   	   $	��350��6%   $	��480��62    � DataContext 5   	   ��  �� RelativeSource �� *Self+ � 	Resources 5
      :   4WorldEditor.Helpers.Converters.IdToI18NTextConverter   IdToI18N;   5WorldEditor.Helpers.Converters.IsGreaterThanConverter  GreaterThan7   1WorldEditor.Search.Items.ColorByOperatorConverter  ColorByOperatorConverter��%W   &	        &	      &	 &       5   
   6'     5   
   6(     5   
   6(   5      .��5      �  � ColumnDefinitions 5   
   �� 5      �� 5      $
��0.5* �6   �� 5      $
��0.5* �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      .+�5   
   ��  �� SelectedValuePath$ Id��6   $��0��6)   $��7��69   $��3q�6J   ��6U   �� *EffectsSource+ �� SelectedValue 6{   �� *EffectToEdit.EffectId+��5      �� 5        �� DataType$ d2OClasses:Effect��6   .R�5!      �� 	 �� Orientation$	 
Horizontal=�6!   .+�5"      �� $��3q�6$   ��6/   �� *DescriptionId+
 �� 	Converter#
 [  ��6s   �� *
Operator+#
 [ �� 5#      $��3q�6$   $��10(�6/   ��6G   �� *Id+5$      5%      5'      �� 5(   
   $��Parameter 1 :��6   $��1��6(   $��0��68   �� 5)   
   $��Parameter 2 :��6   $��1��6(   $��1��68   �� 5*   
   $��Parameter 3 :��6   $��1��6(   $��2��68   �� 5+   
   $��Parameter 4 :��6   $��1��6(   $��3��68   �� 5,   
   $��Parameter 5 :��6   $��1��6(   $��4��68   %  Xceed.Wpf.Toolkit.IntegerUpDown  5-   
   $��2��6   $��0��6.   $��3q�6;   �  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Value 6F   �� *EffectToEdit.DiceNum+{�6m   �� *EffectToEdit.ParametersCount+#
 [  �� ConverterParameter$ 0��  5.   
   $��2��6   $��1��6.   $��3q�6;    6F   �� *EffectToEdit.DiceSide+{�6n   �� *EffectToEdit.ParametersCount+#
 [ $ 1��  5/   
   $��2��6   $��2��6.   $��3q�6;    6F   �� *EffectToEdit.Value+{�6k   �� *EffectToEdit.ParametersCount+#
 [ $ 2��  50   
   $��2��6   $��3��6.   $��3q�6;    6F   �� *EffectToEdit[3]+{�6h   �� *EffectToEdit.ParametersCount+#
 [ $ 3��  51   
   $��2��6   $��4��6.   $��3q�6;    6F   �� *EffectToEdit[4]+{�6h   �� *EffectToEdit.ParametersCount+#
 [ $ 4���� 52   
   $��1��6   $��5��6$   $��2��61   $��5q�6E   $��Center=�6P   $
��Wrap=�6m   ��6�   �� *EffectToEdit.Description+��6�   �� *EffectToEdit.Operator+#
 [ A� 53   
   $��Bottom=�6   $��Bottom=�61    A� Rows$ 1��6I   $��1��6R   $��6��6b   $��3q�6o   $��2��6z   .+�54      �� -   5        $��OK��54      $��5q�6"    �� 	IsDefault . 6>   �� 55      -   5        $��Cancel��55       �� IsCancel . $��5q�66   6A   56      57      58      