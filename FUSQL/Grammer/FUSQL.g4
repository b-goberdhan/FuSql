grammar FUSQL;
/*
 * Parser Rules
 */
 query			: command NEWLINE ;
 command		: (find) ;
 find			: FIND groups from ;
 groups			: number GROUPS name USING (column)+ ; // Groups in this case is analogus to clusters
 from			: FROM name ;
 attribute		: (name EQUAL value) | (name NOT_EQUAL value);
 column			: WORD ;
 name			: WORD ;
 number			: NUMBER ;
 value			: WORD | NUMBER ;

/*
 * Lexer Rules
 */
fragment G		: ('G'|'g') ;
fragment R		: ('R'|'r') ;
fragment O		: ('O'|'o') ;
fragment U		: ('U'|'u') ;
fragment P		: ('P'|'p') ;
fragment S		: ('S'|'s') ;
fragment I		: ('I'|'i') ;
fragment N		: ('N'|'n') ;
fragment F		: ('F'|'f') ;
fragment D		: ('D'|'d') ;
fragment M		: ('M'|'m') ;
fragment A		: ('A'|'a') ;
fragment C		: ('C'|'c') ;
fragment E		: ('E'|'e') ;
fragment T		: ('T'|'t') ;
fragment L		: ('L'|'l') ;
UPPERCASE		: [A-Z] ;
LOWERCASE		: [a-z] ;
EQUAL			: '=' ;
NOT_EQUAL		: '!=' ;
GROUPS			: G R O U P S ;
CREATE			: C R E A T E ;
CLASS			: C L A S S ;
FROM			: F R O M ;
USING			: U S I N G ;
FIND			: F I N D ;
AND				: A N D ;
OR				: O R ;
WORD			: (UPPERCASE | LOWERCASE)+ ;
NUMBER			: [0-9]+ ;
NEWLINE			: '\r' '\n' | '\n' | '\r';
WHITESPACE		: (' ')+ -> skip ;