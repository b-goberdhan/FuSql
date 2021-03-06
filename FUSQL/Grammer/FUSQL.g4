grammar FUSQL;
/*
 * Parser Rules
 */
 query			: command NEWLINE ;
 command		: (find) | (check) | (identify) ;
 find			: FIND groups from ;
 groups			: number GROUPS name USING (column)+ ; // Groups in this case is analogus to clusters
 from			: FROM name ;
 where			: WHERE (conditions)+ ;
 get			: GET dataset_info ;
 dataset_info	: INFO ;
 conditions		: (name EQUAL value) | (name NOT_EQUAL value) | 
					(name GREATER_THAN value) | (name LESS_THAN value);
 column			: WORD ;
 name			: WORD ;
 number			: NUMBER ;
 value			: TEXT | NUMBER ;
 string			: STRING;
 check			: CHECK string from ;
 identify		: IDENTIFY string string from ;

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
fragment W		: ('W'|'w') ;
fragment H		: ('H'|'h') ;
fragment K		: ('K'|'k') ;
fragment Y		: ('Y'|'y') ;
UPPERCASE		: [A-Z] ;
LOWERCASE		: [a-z] ;
EQUAL			: '=' ;
NOT_EQUAL		: '!=' ;
GREATER_THAN	: '>' ;
LESS_THAN		: '<' ;
GROUPS			: G R O U P S ;
CREATE			: C R E A T E ;
CLASS			: C L A S S ;
FROM			: F R O M ;
WHERE			: W H E R E ;
GET				: G E T ;
INFO			: I N F O ;
USING			: U S I N G ;
FIND			: F I N D ;
CHECK			: C H E C K ;
IDENTIFY		: I D E N T I F Y ;
AND				: A N D ;
OR				: O R ;
WORD			: (UPPERCASE | LOWERCASE)+ ;
TEXT			: ('\'')(WORD)('\'') ;
NUMBER			: [0-9]+ ;
NEWLINE			: '\r' '\n' | '\n' | '\r';
WHITESPACE		: (' ')+ -> skip ;
STRING			: '\'' (~['"])* '\'';