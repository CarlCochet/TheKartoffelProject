   M S B A M L   `   `   `  ����F  BWorldEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:WorldEditor.Helpers.ConvertersWorldEditor.Helpers.Converters  5   	   M+clr-namespace:WorldEditor.Editors.Files.D2PWorldEditor.Editors.Files.D2P  5   	   -    'WorldEditor.Editors.Files.D2P.D2PEditor   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   =itoc,clr-namespace:WorldEditor.Helpers.Converters     5   	   ;D2P+clr-namespace:WorldEditor.Editors.Files.D2P     5   	   -   5        $	��450��5   	   $	��600��6   $	��600��6"     � Title  61   �� *Title+ � 	Resources 5	      6   0WorldEditor.Helpers.Converters.IsTypeOfConverter   IsTypeOfConverter9   3WorldEditor.Helpers.Converters.EmptyIfZeroConverter  EmptyIfZeroConverter��%:   &	        &	        5
   
   6!     5   
   6$   5      .��5      ]� ��. .+�5   
   A� $	��Top=�6    A� Rows$ 1��6+   $
��Left=�64   .+�5      �� $��3q�6   ��6    �� *AddFileCommand+.��5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��0,0,3,0q�6   $��16��6-   $+��%/WorldEditor;component/Images/add.png��69   $
��Fill=�6h   $��16��6w   �� 5      $
��Wrap=�6     �� TextAlignment$ Center=�64   
Add a file6a   5      5      �� 5      $��3q�6   ��6    �� *RemoveFileCommand+��6F   ��  �� ElementName$ 	FilesGrid�� �� Path$ SelectedItems�.��5      �� $ 
Horizontal=�6   .+�5      �� $��0,0,3,0q�6   $��16��6-   $.��(/WorldEditor;component/Images/delete.png��69   $
��Fill=�6k   $��16��6z   �� 5      $
��Wrap=�6    $ Center=�64   Remove6]   5      5      �� 5      $��3q�6   ��6    �� *ExtractCommand+��6C   �� $ 	FilesGrid��$ SelectedItems�.��5      �� $��Extract to ...��6   $
��Wrap=�62   $ Center=�6F   5      �� 5      $��3q�6   ��6    �� *ExtractAllCommand+.��5      �� $��Extract all to ...��6   $
��Wrap=�66   $ Center=�6J   5       �� 5!      $��3q�6   ��6    �� *SaveCommand+.��5"      �� $
��Save��6   $
��Wrap=�6(   $ Center=�6<   5#      �� 5$      $��3q�6   ��6    �� *SaveAsCommand+.��5%      �� $��Save as ...��6   $
��Wrap=�6/   $ Center=�6C   5&      5'      &   System.Windows.Controls.DataGrid  5(   
   -   5        $��	FilesGrid��5(      $��Bottom=�6$      CanUserAddRows .     CanUserDeleteRows .  	 %� IsVirtualizing	 . 
   AutoGenerateColumns
 .     AlternatingRowBackground �	#FFDCEAF8$   HorizontalScrollBarVisibility$ Disabled=�6     GridLinesVisibility$
 None=�6*     HeadersVisibility$ Column=�6E  $��Stretch=�6`     SelectionUnit$ FullRow=�6~     EnableRowVirtualization .  J� 	AllowDrop . ��6�  �� *Rows+ J� InputBindings 5)      �� 5*      
 �� Key$ Delete��6    �� Command 6*   �� *RemoveFileCommand+ �� CommandParameter 6P   �� $ 	FilesGrid��$ SelectedItems��� 5+      $
 Back��6    6(   �� *RemoveFileCommand+ 6N   �� $ 	FilesGrid��$ SelectedItems�   	CellStyle 5-      �� 5.       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6   .�5/      ��  �� Property � �� �� Value$ 0q�6@   50         Columns 52      4  .System.Windows.Controls.DataGridTemplateColumn  53      ,  &System.Windows.Controls.DataGridColumn   
IsReadOnly .    CellTemplate 54      �� 55      .R�56      �� $��FileTypeIcon��6$   $2��,/WorldEditor;component/Images/page_white.png��68    �� Triggers 57      �� 58   "    �� Value$
 true��6.    �� Binding 6;   ��   �� 	Converter#  [  ! �� ConverterParameter0   *WorldEditor.Editors.Files.D2P.D2PFolderRow"!  .a�59   &   �� " �� 
TargetName$" FileTypeIcon��6-    � ��$. (/WorldEditor;component/Images/folder.png��6_   5:   #   5<      5>      0  *System.Windows.Controls.DataGridTextColumn  5?      #   Header$
# Name��6%    . $   Width5	  /System.Windows.Controls.DataGridLengthConverter$
$ 100*	 6E   1
  +System.Windows.Controls.DataGridBoundColumn% 
  Binding% 6R   �� *Name+  5@      $# 	File Type��6%    . $
$ Auto	 6J   % 6W   �� *Type+  5A      $
# Size��6%    . $
$ Auto	 6E   % 6R   �� *Size+#  [   5B      $# 	Container��6%    . $
$ Auto	 6J   % 6W   �� *	Container+5D      5E      5F      