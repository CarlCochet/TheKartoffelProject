   M S B A M L   `   `   `  ����F  BWorldEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:WorldEditor.HelpersWorldEditor.Helpers  5   	   O,clr-namespace:WorldEditor.Helpers.ConvertersWorldEditor.Helpers.Converters  5   	   C&clr-namespace:WorldEditor.Search.ItemsWorldEditor.Search.Items  5   	   E'clr-namespace:WorldEditor.Editors.ItemsWorldEditor.Editors.Items  5   	   I)clr-namespace:WorldEditor.Helpers.MarkupsWorldEditor.Helpers.Markups  5	   	   C&clr-namespace:WorldEditor.Loaders.I18NWorldEditor.Loaders.I18N  5
   	   *    $WorldEditor.Editors.Items.ItemEditor   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   0bp!clr-namespace:WorldEditor.Helpers     5   	   <clc,clr-namespace:WorldEditor.Helpers.Converters     5   	   7iess&clr-namespace:WorldEditor.Search.Items     5   	   X TXceed.Wpf.Toolkit, Version=1.9.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f45xceed)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   3items'clr-namespace:WorldEditor.Editors.Items  5   	   7markups)clr-namespace:WorldEditor.Helpers.Markups  5	   	   1i18N&clr-namespace:WorldEditor.Loaders.I18N  5
   	     � Icon$<  6/WorldEditor;component/Images/hammer_builder_32xlg.png��5   	    � IsSharedSizeScope . $	��670��5   	   $	��760��6    � 	Resources 5      &    WorldEditor.Helpers.BindingProxy   ModelViewProxy   Data9   3WorldEditor.Helpers.Converters.CurrentLangConverter  CurrentLang6   0WorldEditor.Helpers.Converters.IdToIconConverter  IdToIcon:   4WorldEditor.Helpers.Converters.IdToI18NTextConverter  
I18NToText  BoolToVisibility6   0WorldEditor.Search.Items.ItemEffectStyleSelector  EffectStyleSelector7   1WorldEditor.Search.Items.ColorByOperatorConverter  ColorByOperatorConverter8   2WorldEditor.Helpers.Converters.LangToFlagConverter  
LangToFlag��%�   &	        &	      &	 /     &	 B     &	 U     &	 h     &	 {     &	 �       5   
    61   ��   5   
   6#     5   
   6      5   
   6$   �� 5   
   6'     5   
   6'     5   
   6(     5   
   6"   5       � Title 5      �� 5   
    �� Path$ 	Item.Name�6    �� StringFormat$ Editing '{0}'��6#   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5       $
��Auto �6   �� 5!      $
��Auto �6   �� 5"      �� 5#      $��2* �6   �� 5$      $
��Auto �6   .+�5&   
   ]� $��0��6   ��. .+�5'      �� $
��Left=�6   ��5(      �� 5)       �� 
TargetType" ��6   	 �� Triggers	 5*      P� 5+      
 P� Property
 �@J�IsMouseOver P� Value . . �5,   "   ��  �� Property � �� �� Value# � ��6F   �� 5-   "    �@�Cursor$
 Hand��6L   5.      50      ._�52      �� .+�53      �� $
��Left=�6   $��Center=�68   $��3q�6S   ��6^   �� *ChangeIconCommand+��54      �� 55       �� 
TargetType" ��6.   .R�56   "   �� $
��None=�6(   ��67   �� *Item.IconId+ �� 	Converter# [ 57      59      �� 5:      $��8(�6    $��Center=�67   $��0,0,0,3q�6T   ��6e   �� *Item.IconId+5;      5<      �� 5=      ���	#FF4682B4$��40(�6-   $��0q�6;   ���	#00FFFFFF$��Bottom=�6f   $	��Top=�6�   $��0q�6�   $��3,3,3,0q�6�   ��6�   �� *	Item.Name+�� 5>      -   5        $��ItemIdField��5>      ���	#FF696969$��25(�6@   $��Center=�6N   $��Bottom=�6i   {�.  $��3,10,3,9q�6�   ��6�   �� *	Item.Id+�� 5?      -   5        $��20��5?      $��25��6    $
��2,17q�6,   .��5@      �� $��16��6   $8��2/WorldEditor;component/Images/pencil_003_16xmd.png��6$   $��16��6`   5A      �� 5B      $��Right=�6   $��Center=�63   $��5q�6N    �� SelectedValue 6Y   �� *Item.CurrentLanguage+��6�   ��  �� Source 5   /WorldEditor.Helpers.Markups.EnumValuesExtension  *M� *i18N:Languages++��5C      �� 5D       �� DataType$ i18N:Languages��6#   .R�5E      �� $��3q�6%    �� Orientation$ 
Horizontal=�60   .+�5F      �� $��3q�6$   $��Center=�6/   ��6L   �� # [ �� 5G      $��3q�6(   $��Center=�63   ��6P   �� 5H      5I      5K      5L      �� 5M   
   $��Primary Properties��6   $��1��6/   .��5N      �  5O      �� 5P      �� 5Q      �� 5R      �� 5S       � ColumnDefinitions 5U      �� 5V      $
��0.5* �6'    z� SharedSizeGroup$ Label��64   �� 5W      �� 5X      $
��0.0* �6'   $ Label��64   �� 5Y      $
��1.5* �6'   �� 5Z      $
��0.5* �6'   $ Label��64   �� 5[      .+�5]      �� $��Level��6   $
��Left=�6)   $��Center=�6D   $��3q�6_   %	  Xceed.Wpf.Toolkit.IntegerUpDown	  5^      $��1��6&   �
  �Xceed.Wpf.Toolkit.NumericUpDown`1[[System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]] 
  Minimum$ 1X�66    
  Maximum$	 200X�6B   $��3q�6P   �  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Value 6[   �� *
Item.Level+�� 5_      $
��Type��6   $��2��6(   $��3q�68   �� 5`      $��3��6   $��3q�6+    �� SelectedValuePath$ Id��66   ��6M   �� *Types+ 6k   �� *Item.TypeId+��5a      �� 5b      $ d2OClasses:ItemType��6'   .R�5c      �� ��6(   �� *NameId+# [ 5d      5f      �� 5g      $��Weight��6   $��4��6*   $��3q�6:   	  5h      $��5��6&   $ 0X�66   $��3q�6B    6M   �� *Item.Weight+�� 5i      $��Price��6   $��1��6)   $��0��66   $��3q�6F   	  5j      $��1��6&   $��1��63   $ 0X�6C   $��3q�6O    6Z   �� *
Item.Price+�� 5k      $��Item Set��6   $��1��6,   $��2��69   $��3q�6I   ]� 5l      $��1��6   $��3��6)   .+�5m      �� $��Right=�6   $��-1,3,3,3q�64   $��22��6F   ��6Q   �� *ResetItemSetCommand+.��5n      �� $;��5/WorldEditor;component/Images/action_cancel_16xlg.png��6    5o      �� 5p      $��3q�6   $ Id��6*   ��6A   �� *
ItemSets+ 6b   �� *Item.ItemSetId+��5q      �� 5r      $ d2OClasses:ItemSet��6+   .R�5s   "   �� ��6,   �� *NameId+# [ 5t      5v      5w      �� 5x      $��
Appearance��6   $��1��6.   $��4��6;   $��3q�6K   	  5y      $��1��6&   $��5��63   $ 0X�6C   $��3q�6O    6Z   �� *Item.AppearanceId+�� 5z      $��Criteria��6   $��2��6,   $��0��69   $��3q�6I   �� 5{      $��2��6   $��1��6'   $��3q�67   ��6B   �� *Item.Criteria+�� 5|      $��3��6   $ 
Horizontal=�6*   $��4��6C   $��0��6W   $��3q�6g   .+�5}      �� $��
Two handed��6   $��3q�64    d� 	IsChecked 6?   �� *Item.TwoHanded+�� 5~      $��Hide effects��6   $��3q�66    6A   �� *Item.HideEffects+�� 5      $��Cursed��6   $��3q�60    6;   �� *Item.Cursed+5�      5�      5�      �� 5�   
   $��2��6   ��5�      �� 5�      $��Usable item��6    61   �� *Item.Usable+.��5�      �  5�      �� 5�      �� 5�       5�      �� 5�      $
��0.5* �6'   $ Label��64   �� 5�      �� 5�      �� 5�      $
��0.5* �6'   �� 5�      .+�5�      �� $��Use Animation��6   $
��Left=�61   $��Center=�6L   $��3q�6g   	  5�      $��1��6&   $ 0X�66   $��3q�6B    6M   �� *Item.UseAnimationId+{�6s   �� *Item.Usable+�� 5�      $��
Targetable��6   $��2��60   $��3q�6@   $��Center=�6K    6f   �� *Item.Targetable+{�6�   �� *Item.Usable+�� 5�      $��Target Criteria��6   $��0��63   $��3��6@   $��3q�6P   �� 5�      $��0��6   $��4��6'   $��3q�67   ��6B   �� *Item.CriteriaTarget+{�6g   �� *Item.Targetable+�� 5�      $��Non Usable On Another��6   $��1��6;   $��2��6H   $��0��6\   $��3q�6l    6w   �� *Item.NonUsableOnAnother+{�6�   �� *Item.Usable+5�      5�      �� 5�   
   $��Weapon Properties��6   $��3��6.   y�6;   �� *
IsWeapon+# [ .��5�      �  5�      �� 5�      �� 5�      �� 5�       5�      �� 5�      $
��0.5* �6'   $ Label��64   �� 5�      �� 5�      $
��0.5* �6'   $ Label��64   �� 5�      �� 5�      $
��0.5* �6'   $ Label��64   �� 5�      .+�5�      �� $��Ap Cost��6   $
��Left=�6+   $��Center=�6F   $��3q�6a   	  5�      $��1��6&   $ 0X�66   $��3q�6B    6M   �� *Item.ApCost+�� 5�      $��	Min Range��6   $��2��6-   $��3q�6=   	  5�      $��3��6&   $ 0X�66   $��3q�6B    6M   �� *Item.MinRange+�� 5�      $��Range��6   $��4��6)   $��3q�69   	  5�      $��5��6&   $ 0X�66   $��3q�6B    6M   �� *
Item.Range+�� 5�      $��Critical Hit Probability��6   $��1��6<   $��0��6I   $��3q�6Y   	  5�      $��1��6&   $��1��63   $ 0X�6C   $��3q�6O    6Z   �� *Item.CriticalHitProbability+�� 5�      $��Critical Hit Bonus��6   $��1��66   $��2��6C   $��3q�6S   	  5�      $��1��6&   $��3��63   $ 0X�6C   $��3q�6O    6Z   �� *Item.CriticalHitBonus+�� 5�      $"��Critical Failure Probability��6   $��1��6@   $��4��6M   $��3q�6]   	  5�      $��1��6&   $��5��63   $ 0X�6C   $��3q�6O    6Z   �� *" Item.CriticialFailureProbability+�� 5�      $��2��6   $ 
Horizontal=�6*   $��4��6C   $��3q�6W   .+�5�      �� $��Cast in Line��6   $��2��66   $��0��6C   $��3q�6S    6^   �� *Item.CastInLine+�� 5�      $��Cast in Diagonal��6   $��2��6:   $��1��6G   $��3q�6W    6b   �� *Item.CastInDiagonal+�� 5�      $��Cast Test Los��6   $��2��67   $��2��6D   $��3q�6T    6_   �� *Item.CastTestLos+5�      5�      5�      �� 5�   
   $��Description��6   $��4��6(   .��5�      ��  �� TextWrapping$
 Wrap=�6    �� AcceptsReturn . ��6?   �� *Item.Description+5�      �� 5�   
   $��Effects��6   $��5��6$   .��5�      ]� .+�5�      c� $	��Top=�6    5�      ��%�   '��       �� 5�      " ��67   	 5�      P� 5�   "   
 � {� .  . �5�   &   ��  �@J�Opacity$	 0.4Q�6J   5�   #   5�      5�      .@�5�      �� $��2q�6   ��6(   �� *EditEffectCommand+��6N   �� *SelectedItem+ �� ElementName$ EffectsListBox��.��5�      �� $��16��6    $8��2/WorldEditor;component/Images/pencil_003_16xmd.png��6,   $��16��6h   5�      �� 5�      $��2q�6   ��6(   �� *AddEffectCommand+.��5�      �� $��16��6    $8��2/WorldEditor;component/Images/action_add_16xlg.png��6,   $��16��6h   5�      �� 5�      $��2q�6   ��6(   �� *RemoveEffectCommand+��6P   �� *SelectedItems+$ EffectsListBox��.��5�      �� $��16��6    $;��5/WorldEditor;component/Images/action_cancel_16xlg.png��6,   $��16��6k   5�      5�      �� 5�      -   5        $��EffectsListBox��5�      $��2��60   $��2��6=   #��[ 6Q   $��Stretch=�6�   $��Stretch=�6�    �� SelectionMode$ Extended=�6�   ��6�   �� *Item.WrappedEffects+��5�      �� 5�      -   'WorldEditor.Editors.Items.EffectWrapper"  6'   .R�5�      �� ��6(   �� *Description+��6E   �� *
Operator+# [ 5�      5�      5�      5�      �� 5�   
   $��6��6   $��5q�6   ��6)   �� *SaveCommand+.��5�      �� $ 
Horizontal=�6   .+�5�      �� $1��+/WorldEditor;component/Images/save_6530.png��6   $��16��6M   $��16��6X   $��3q�6d   �� 5�      $
��Save��6   $��3q�6(   $��14(�63   5�      5�      5�      5�      