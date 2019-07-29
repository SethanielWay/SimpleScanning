#Mark Forster System (Square Space Blog)

-Long List System
	-Simple Scanning
	
-Actions
	-Cross Out an Item
	-Choose active item
		-when done either
			-Cross out
			-or re-enter at end of list
			-or skip to end of list
			
-Must have
	-Console UI
	-Enter user-provided tasks, free form text
		-addable at any time, always last item on the "long list'
	-Pages of items, should contain 20 items each
	-view by page: 
		1st Page begins w/ first unactioned item
		Actioned items following unactioned items are preserved and displayed
		Pages containing no unactioned items are preserved and displayed
		actioned items before 1st unactioned item are deleted
	-completed items should be notated by grey text
	-current item of work should be highlighted(invert foreground and background colors)
	-data stored in flat-text file in the current directory, pre-defined filename ("tasks")
	
-stretch goals
	-"item info" e.g. entry date, or number of reentries (which diplays 
	separately from the task list)