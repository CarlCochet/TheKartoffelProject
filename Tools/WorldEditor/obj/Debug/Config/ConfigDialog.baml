   M S B A M L   `   `   `  ����F  BWorldEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:WorldEditor.HelpersWorldEditor.Helpers  5   	   7 clr-namespace:WorldEditor.ConfigWorldEditor.Config  5   	   %    WorldEditor.Config.ConfigDialog   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   0dr!clr-namespace:WorldEditor.Helpers     5   	   0per clr-namespace:WorldEditor.Config     5   	     � Icon$2  ,/WorldEditor;component/Images/gear_32xlg.png��5   	    � SizeToContent$ WidthAndHeight=�5   	    � Title$ Configure WorldEditor��6(   $	��340��5   	   $	��700��6   $	��500��6%    � 	Resources 5
         Bool2Visibility��%   &	        �� 5   
   6'   5      .��5      �  � ColumnDefinitions 5   
   �� 5      �� 5       � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��2��6!    �� TextAlignment$ Center=�65   $��Stretch=�6V   $��Stretch=�6t   $��8q�6�   $��14(�6�   $
��Wrap=�6�   y�6�   �� *IsFirstLaunch+ �� 	Converter# [  b`Before using the WorldEditor for the first time, please be sure the following settings are valid6z  �� 5   
   $��Database��6   $��1��6%   .��5      �� .+�5      �  5      �� 5      �� 5      �� 5      �� 5      �� 5        5"      �� 5#      �� 5$      .+�5&      �� $��0��6    $��0��6-   $
��Host��6=   $
��Left=�6I   $��Center=�6d   �� 5'      $��1��6    $��0��6-   $
��User��6=   $
��Left=�6I   $��Center=�6d   �� 5(      $��2��6    $��0��6-   $��Password��6=   $
��Left=�6M   $��Center=�6h   �� 5)      $��3��6    $��0��6-   $��Database Name��6=   $
��Left=�6R   $��Center=�6m   �� 5*      $��4��6    $��0��6-   $��
SQL Engine��6=   $
��Left=�6O   $��Center=�6j   �� 5+      $��0��6   $��3q�6+   $��1��66   $	��Top=�6F   ��6^   �� *DBConfig.Host+�� 5,      $��1��6   $��3q�6+   $��1��66   $	��Top=�6F   ��6^   �� *DBConfig.User+�� 5-      $��2��6   $��3q�6+   $��1��66   $	��Top=�6F   ��6^   �� *DBConfig.Password+�� 5.      $��3��6   $��3q�6+   $��1��66   $	��Top=�6F   ��6^   �� *DBConfig.DbName+�� 5/      $��4��6   $��3q�6,   $��1��67   $	��Top=�6G    �� SelectedValuePath$ InvariantName��6_   ��6�   �� *	Factories+	 �� SelectedValue	 6�   �� *DBConfig.ProviderName+��50      �� 51      
 �� DataType$!
 config:DbFactoryInformation��6+   .R�52   "   �� ��6,   �� *Name+53      55      56      �� 57      $��1��6   $��3q�6)   ��64   �� *TestDBCommand+.��58      ��  �� Orientation$ 
Horizontal=�6!   .+�59      �� $9��3/WorldEditor;component/Images/Relation_8467_32x.png��6    $��16��6]   $��16��6h   $	��3,0q�6t   �� 5:      $��Try connection��6$   5;      5<      5=      5>      �� 5?   
   $��Loaders��6   $��1��6$   $��1��64   .��5@      �  5A      &    WorldEditor.Helpers.DataResource  BasePath   BindingTarget �� 
TargetType �� Triggers P� Property P� Value �� Property �� Value �� RelativeSource �� Path��%  &	       '�� 8       5B       67   �� *LoaderSettings.BasePath+�� 5C      " ��65    5D      P� 5E       �@=�HasError . . �5F   "   ��  �@�ToolTip 6M   ��  �� *Self+$) #(Validation.Errors)[0].ErrorContent�5G      5I      5J       5K      �� 5L      $
��Auto �6$   �� 5M      $
��Auto �6$   �� 5N      $
��Auto �6$   �� 5O      $
��Auto �6$   �� 5P      $
��Auto �6$   �� 5Q      $
��Auto �6$   �� 5R      $
��Auto �6$   �� 5S      $
��Auto �6$    5U      �� 5V      $
��0.5* �6'   �� 5W      .+�5Y      �� $��0��6   $��0��6)   $��Dofus App. Path��69   $
��Left=�6P   $��Center=�6k   �� 5Z      $��1��6   $��0��6)   $��	Maps File��69   $
��Left=�6J   $��Center=�6e   �� 5[      $��2��6   $��0��6)   $��D2O Directory��69   $
��Left=�6N   $��Center=�6i   �� 5\      $��3��6   $��0��6)   $��D2I Directory��69   $
��Left=�6N   $��Center=�6i   �� 5]      $��4��6   $��0��6)   $��Item Icons File��69   $
��Left=�6P   $��Center=�6k   �� 5^      $��5��6   $��0��6)   $��World Gfx File��69   $
��Left=�6O   $��Center=�6j   �� 5_      $��6��6   $��0��6)   $��World Ele File��69   $
��Left=�6O   $��Center=�6j   �� 5`      $��7��6   $��0��6)   $��Map Decr. Key��69   $
��Left=�6N   $��Center=�6i   �� 5a      $��0��6   $��3q�6'   $��1��62   $	��Top=�6B   .��6Z   ��5b      �� 5c      $ LoaderSettings.BasePath�6"    �� ValidationRules 5d      '   !WorldEditor.Config.PathExistsRule  5e   "      IsDirectory . 65   5g      .��5h   $   5i      �� 5j      $��1��6   $��3q�6'   $��1��62   $	��Top=�6B   .��6Z   ��5k      �� 5l      $% LoaderSettings.MapsRelativeFile�6"    5m        5n   "    .     BasePath 6I   6   0WorldEditor.Helpers.DataResourceBindingExtension     DataResource# [ 5p      .��5q   $   5r      �� 5s      $��2��6   $��3q�6'   $��1��62   $	��Top=�6B   .��6Z   ��5t      �� 5u      $) #LoaderSettings.D2ORelativeDirectory�6"    5v        5w   "    .  6H     # [ 5y      .��5z   $   5{      �� 5|      $��3��6   $��3q�6'   $��1��62   $	��Top=�6B   .��6Z   ��5}      �� 5~      $) #LoaderSettings.D2IRelativeDirectory�6"    5        5�   "    .  6H     # [ 5�      .��5�   $   5�      �� 5�      $��4��6   $��3q�6'   $��1��62   $	��Top=�6B   .��6Z   ��5�      �� 5�      $* $LoaderSettings.ItemIconsRelativeFile�6"    5�        5�   "    .  6H     # [ 5�      .��5�   $   5�      �� 5�      $��5��6   $��3q�6'   $��1��62   $	��Top=�6B   .��6Z   ��5�      �� 5�      $) #LoaderSettings.WorldGfxRelativeFile�6"    5�        5�   "    .   6I     # [ 5�      .��5�   $   5�      �� 5�      $��6��6   $��3q�6'   $��1��62   $	��Top=�6B   .��6Z   ��5�      �� 5�      $) #LoaderSettings.WorldEleRelativeFile�6"    5�        5�   "    .   6I     # [ 5�      .��5�   $   5�      �� 5�      $��7��6   $��3q�6'   $��1��62   $	��Top=�6B   ��6Z   �� *(&LoaderSettings.GenericMapDecryptionKey+5�      5�      A� 5�   
   $��2��6   $��1��6#    A� Rows$ 1��63   $��Bottom=�6<   .+�5�      �� -   5        $��OK��5�       �� 	IsDefault . $��3q�63   6>   �� 5�      -   5        $��Cancel��5�       �� IsCancel . $��3q�66   ��5�      �� 5�      " ��6    5�      �� 5�       �� Binding 6*   �� *IsFirstLaunch+ �� Value True6L   .a�5�   "   ��  � {� .  6H   5�      5�      5�      5�      5�      5�      